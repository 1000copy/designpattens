2018年01月25日 ，做代码的全复杂度报告，找到了这样的一段代码，项目组认为需要修改，提交给我，希望我协助支持，找到比较好的重构方向和方法。
这个项目是一个网店服务，它需要从淘宝等电商平台下载商品并做保存。

这段代码，确实单看指标并不算大：

1. 圈复杂度不过18
2. 代码行也真不算多
3. 代码块也就是初略的有三块

然而，很难看懂，我看的颈椎病都犯了:),因为：

1. existedMapping的查找感觉在GetEShopProductSkuMappingExisted做了一遍，在SaveEShopProductSkuMapping内也做了一遍，代码雷同
2. textilesPropId看起来什么也没做
3. EShopProductSkuMapping不知道代表的是什么

代码就是这样的：

    public void SaveEShopProductSkuMapping(DbHelper dbHelper, EShopProductSkuMapping mapping, ulong textilesPropId)
    {
        List<ulong> syncSkuIds = new List<ulong>();
        EShopProductSkuMapping existedMapping = GetEShopProductSkuMappingExisted(mapping);
        if (null == existedMapping)
        {
            if (LocalEShopProductSkuMapping != null && LocalEShopProductSkuMapping.Count > 0 && !string.IsNullOrEmpty(mapping.PlatformNumId) && !string.IsNullOrEmpty(mapping.PlatformSkuId))
            {
                //isSkuExist = EShopProductSkuMappingSqlManager.CheckSkuExist(dbHelper, ProfileId, EShopId, mapping.PlatformNumId, mapping.PlatformSkuId);
                existedMapping = LocalEShopProductSkuMapping.FirstOrDefault(m => mapping.PlatformNumId == m.PlatformNumId && mapping.PlatformSkuId == m.PlatformSkuId);
            }
            if (null == existedMapping)
            {
                bool isSuccess = InsertProductSkuMapping(dbHelper, mapping);
                if (isSuccess && _operateType == ProductMappingLogOpreateType.Refresh)
                {
                    _logger.InsertLog(existedMapping, mapping, ProductMappingLogOpreateType.Refreash);
                }
                return;
            }
        }
        mapping.Id = existedMapping.Id;
        if (textilesPropId > 0) //家纺套餐行标记不删除
        {
            var textilesMapping = LocalEShopProductSkuMapping.FirstOrDefault(x =>
                x.PlatformNumId == mapping.PlatformNumId && x.PlatformSkuId == existedMapping.PlatformRSkuId);
            MarkProductSkuMappingAsNotRedundant(textilesMapping);
        }
        if (!IsSkuNeedRefresh(mapping, existedMapping))
        {
            MarkProductSkuMappingAsNotRedundant(existedMapping);
            return;
        }
        if (IsMappingChanged(existedMapping, mapping))
        {
            syncSkuIds.AddRange(SkuGetterUtil.GetSkuIds(dbHelper, mapping, _isMatchPtypeByXcode));
        }
        //需要将新生成的skuid赋值给rskuid, 否则不能完成对应
        //mapping.PlatformRSkuId = existedMapping.PlatformRSkuId;//存在的rskuid不清空
        bool isModSucess = ModifyProductSkuMapping(dbHelper, mapping);
        if (isModSucess && _operateType == ProductMappingLogOpreateType.Refreash && _isMatchPtypeByXcode &&
            IsMappingChanged(existedMapping, mapping))
        {
            _logger.InsertLog(existedMapping, mapping, ProductMappingLogOpreateType.Refreash);
        }
        MarkProductSkuMappingAsNotRedundant(existedMapping);
        _logger.DoMarkQtyChange(dbHelper, ProfileId, syncSkuIds);
    }
    public EShopProductSkuMapping GetEShopProductSkuMappingExisted(EShopProductSkuMapping skuMapping)
    {
        if (null == LocalEShopProductSkuMapping || LocalEShopProductSkuMapping.Count == 0 || null == skuMapping)
        {
            return null;
        }
        return LocalEShopProductSkuMapping.FirstOrDefault(m => IsSameSku(skuMapping, m));
    }
    
经过和项目组同事的结对协作，发现这段代码大概几个问题被清理出来：
1. textilesPropId其实是废掉的代码。曾经想要加入的业务，后来又不加了，半拉子工程也没有被清理，放在了这里
2. EShopProductSkuMapping是一个电商下载的商品类，当然名字略微不好接受，但是看了代码的实现类，我知道它怎么回事即可，暂时和本重构无关，不需要深究

其实这段代码，可以使用”查询和操作分离“的手法，在忽略写日志的之外，了解到它的操作就是做保存，为了保存，会根据existedMapping的情况，或者ModifyProductSkuMapping，伪代码如下：

    if(someCondition())
      InsertProductSkuMapping(dbHelper, mapping)
    else
      ModifyProductSkuMapping(dbHelper, mapping)

这一段代码：

    if (!IsSkuNeedRefresh(mapping, existedMapping))
    {
        MarkProductSkuMappingAsNotRedundant(existedMapping);
        return;
    }

和SaveProductSkuMapping的职责无关，故而，要么上浮此代码块（pull up）到调用者（调用SaveProductSkuMapping的函数），要么不懂当前SaveProductSkuMapping函数，而把真正的SaveProductSkuMapping的保存职责下沉到一个新函数（push down）。效果一样的。

大体步骤就是：
1. 合并本该在一起的existedMapping的逻辑
2. 删除无用代码和无用标注
3. 把真正的保存代码，其中涉及到修改或者插入对象的操作，已经相应的log都放到一起，然后把这个代码块下沉到新的函数内

无编译器保护，缺乏业务清晰的情况下，仅仅通过等效变换的方式是比较难做重构的，因此这里的是大概步骤，是思路，不是最终解决方案。为了避免啰嗦，我录制了一个视频，放在这里（需要梯子）：

    https://youtu.be/cVQqmV8mV-0

视频10分钟的时候，有一小段被别的事儿打断，录屏是停止的，可以快进之。
总结：

1. 我得继续加强学习业务
2. 我会减少评论，尽量展示，点到为止

下一步：

1. 思想分析。在重构过程中，查看系统的代码，发现很多类似的bad smell：极长函数名，其中包含了动词加名词的结构的函数，体现了过程性极强的，对象职责分离极弱的思想。此处的bad smell是一个重构的方向。一个猜想
2. 继续说对象职责分离。单单以此次重构的函数SaveEShopProductSkuMapping。好像此工作应该从当前类移动到EShopProductSkuMapping类内，作为EShopProductSkuMapping.save为妥当
3. 如果EShopProductSkuMapping不允许去修改怎么办？可以使用Decorator模式，在不修改此代码的前提下，为此类提供新的功能。总之办法是有的。
4. 命名空间的利用。EShopProductSkuMapping的命令加入EShop前缀，此种情况也不少，感觉要么是冗余的，要么就是namespace这个技术特性利用的不够。一个猜想
5. 名词缩小，从EShopProductSkuMapping到Sku。去掉前缀，去掉无意义的Mapping ,名字就是ProductSku，其实，直接就叫做SKU也是可以的。因为SKU本来就是最小商品的概念，因此product也无必要。


