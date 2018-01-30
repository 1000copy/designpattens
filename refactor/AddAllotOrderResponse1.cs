public override AddAllotOrderResponse Invoke(AddAllotOrderRequest request)
{
    int i = 0;
    bool isChange = false;//是否对比变化量

    var response = new AddAllotOrderResponse();
    if (request == null)
    {
        response.IsSuccess = false;
        response.ErrorMsg = "请求参数为空！";
        return response;
    }
    using (DbHelper dbHelper = DbCreator.CreateDbHelper(request.ProfileId))
    {
        try
        {
            if (request.SaveEntity == null || request.SaveEntity.lstDetailPTypes == null || request.SaveEntity.lstDetailPTypes.Count == 0)
            {
                throw new MessageException("要保存的调拨单无数据");
            }

            //处理单号和账套ID
            SetOrderProfileId(request);

            isChange = IsChangeOrder(request);

            string errPtypesStr = CheckGoodsStock(dbHelper, request, isChange, out i);
            if (!string.IsNullOrEmpty(errPtypesStr))
            {
                string tmpMsg = i >= 5 ? errPtypesStr.Trim(new char[] { ';', '\r', '\n' }) + " 等;\r\n" : errPtypesStr;
                response.IsSuccess = false;
                response.ErrorMsg = "\r\n以下商品存在负库存：\r\n" + tmpMsg;
                return response;
            }

            //库存变化情况下，保存过账记录 
            RetailBack.Entity.Bill.GoodsTransBillEntity billEntity = new RetailBack.Entity.Bill.GoodsTransBillEntity();
            var billContext = new BillContext(dbHelper, request.ProfileId, request.EtypeId);
            var sInAction = new GoodsTransBillAction(billContext);
            CreateBillEntity(billEntity, request.SaveEntity);

            var res = sInAction.Save(billEntity);
            if (res.ResultType != BillPostingResultType.Success)
            {
                throw new MessageException(string.Format("过账调拨计划单出错，错误：{0}", res.Message));
            }
            response.IsSuccess = true;
            response.ErrorMsg = null;
        }
        catch (Exception ex)
        {
            throw new MessageException(string.Format("保存调拨计划单出错，错误：{0}", ex.Message));
        }
    }
    return response;
}

private string CheckGoodsStock(DbHelper dbHelper, AddAllotOrderRequest request, bool isChange, out int times)
{
    times = 0;
    bool isF = true;
    string res = string.Empty;
    var errPtypes = new StringBuilder();

    DlyTransEntity itemEntity = null;
    var isNew = request.SaveEntity.IsNewItem;
    //取到是否允许负库存的标识
    var subV = SysDataSqlManager.GetSysDataSubValue(dbHelper, request.ProfileId, "retail_allowNegativeOnHand");
    bool allow = subV == "0" ? false : true;
    //不允许负库存时进入，判断是否产生负库存，如产生负库存则抛出异常
    if (!allow && isNew)
    {
        bool result = false;
        for (int i = 0; i < request.SaveEntity.lstDetailPTypes.Count; i++)
        {
            if (i >= 5)//控制到5次
            {
                break;
            }

            itemEntity = request.SaveEntity.lstDetailPTypes[i];

            result = AllotOrderSqlManager.CheckGoodsStock(dbHelper, request.ProfileId, request.SaveEntity.StoreId, itemEntity, isChange, ref isF);
            if (!result)
            {
                res = string.Empty;
                res = SetResultMsg(itemEntity);
                errPtypes.Append(res);
            }
        }
    }

    return errPtypes.ToString();
}

private string SetResultMsg(DlyTransEntity itemEntity)
{
    string res = string.Empty;
    var sbFormat = new StringBuilder();

    if (!string.IsNullOrEmpty(itemEntity.Prop1Value))
    {
        sbFormat.Append(itemEntity.PFullName + "-" + itemEntity.Prop1Value + "-");
    }
    else
    {
        sbFormat.Append(itemEntity.PFullName + "-");
    }

    if (!string.IsNullOrEmpty(itemEntity.Prop2Value))
    {
        sbFormat.Append(itemEntity.Prop2Value + "-");
    }
    if (!string.IsNullOrEmpty(itemEntity.Prop3Value))
    {
        sbFormat.Append(itemEntity.Prop3Value + "-");
    }
    res = sbFormat.ToString().Trim('-') + ";\r\n";

    return res;
}