## PtypeSelectorPageHelper 优化报告

1. 对方法GetPTypeInfo(IHashObject info)的重构  

该方法主要是用于在切换赠品时，获取基本信息数据
但是在该方法中，参杂了大篇幅的sql语句，判断语句，并且职能不单一，故对其进行了优化，做到职责单一，方便阅读

  a. 将获取商品信息部分提取出来，封装成一个方法  

  b. 将获取库存数据部分提取出来，封装成了一个方法 

  c. 在GetPtypeInfo方法里，用这两个方法替换掉相应的代码

2. 对方法QueryPTypeInfoList(ref string filterstr, IHashObject info,
            bool isShowAssistantUnit, bool userFullBarCode, bool isFilterPublish, bool isStop)进行重构   

该方法的目的时为了查询Ptypeinfo的一个集合，根据多个参数进行选择、组合查询
但是在该方法中，存在大篇幅的if语句，sql脚本，以及职责臃肿  
所以对其进行了优化，要做到减少if的嵌套，职责单一  

a.将根据model的值来进行判断的if..else..修改为switch语句，这样观看起来更加直接  

b.将每个case里面的代码进行一个封装，这样确保职责的单一，并且良好的命名可以让读者一目了然，知道这个case的作用是什么  

c.将大篇幅的sql提取出来，避免一个函数过长

3. 对QueryPTypeInfosCount(PTypeSelectorParams queryParams, bool hideStoppedItem,
            ulong storeid)函数进行优化  

该函数主要时查询ptype商品信息的数量，但是由于查询的参数是选择性的，所以导致代码比较混杂。
同样的在该方法中存在了大量的if语句嵌套

a.由于模糊查询的if中占用了大篇幅的代码，所以先将该部分提取出来，GetPtypeInfoFuzzyQuerySql；在GetPtypeInfoFuzzyQuerySql中，有部分是根据filtermode的值来进行选择执行哪一种操作的，所以使用switch来替换if更好一些

4. 对QueryPTypeInfos(PTypeSelectorParams queryParams, bool hideStoppedItem,
            ulong storeid, int vchtype, bool isInventory)方法进行重构

该方法主要是查询PtypeInfo的集合，该方法主要分为4个部分，  
1.获取是不是publishfilterstr
2.获取排序部分的sql
3.获取ptype的过滤部分的sql
4.根据之前的值，组合sql并进行查询

所以本函数的优化就是根据这4个步骤进行提取函数，并且比较简单的一些if语句使用三目运算符进行替代，使代码清晰明了，并且做到了职责单一，一个函数只做一件事情


