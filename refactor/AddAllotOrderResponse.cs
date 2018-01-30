public override AddAllotOrderResponse Invoke(AddAllotOrderRequest request)
{
    bool isChange = false;//是否对比变化量
    bool isF = true;
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
            if (request.SaveEntity == null)
            {
                response.IsSuccess = false;
                response.ErrorMsg = "要保存的调拨单为空！";
                return response;
            }
            //处理单号和账套ID
            request.SaveEntity.ProfileId = request.ProfileId;
            //request.SaveEntity.Number = VchTypeHelper.GetAutoVchNumber(dbHelper, request.ProfileId, (int)ApiVchTypes.GoodsTransBill, true);
            foreach (var item in request.SaveEntity.lstDetailPTypes)
            {
                item.ProfileId = request.ProfileId;
                //只要存在一个变化的Qty就认为是修改单据
                if (item.OutChangeQty != 0)
                {
                    isChange = true;
                }
            }

            int i = 0;
            StringBuilder sbFormat = new StringBuilder();
            var isNew = request.SaveEntity.IsNewItem;
            //取到是否允许负库存的标识
            var subV = SysDataSqlManager.GetSysDataSubValue(dbHelper, request.ProfileId, "retail_allowNegativeOnHand");
            bool allow = subV == "0" ? false : true;
            //不允许负库存时进入，判断是否产生负库存，如产生负库存则抛出异常
            if (!allow && isNew)
            {
                bool result = false;
                var errPtypes = new StringBuilder();
                foreach (var item in request.SaveEntity.lstDetailPTypes)
                {
                    result = AllotOrderSqlManager.CheckGoodsStock(dbHelper, request.ProfileId, request.SaveEntity.StoreId, item, isChange, ref isF);
                    if (!result)
                    {
                        if (i >= 5)//控制到5次
                        {
                            break;
                        }
                        i++;
                        sbFormat.Clear();
                        if (!string.IsNullOrEmpty(item.Prop1Value))
                        {
                            sbFormat.Append(item.PFullName + "-" + item.Prop1Value + "-");
                        }
                        else
                        {
                            sbFormat.Append(item.PFullName + "-");
                        }

                        if (!string.IsNullOrEmpty(item.Prop2Value))
                        {
                            sbFormat.Append(item.Prop2Value + "-");
                        }
                        if (!string.IsNullOrEmpty(item.Prop3Value))
                        {
                            sbFormat.Append(item.Prop3Value + "-");
                        }
                        errPtypes.Append(sbFormat.ToString().Trim('-') + ";\r\n");
                    }
                }

                if (!string.IsNullOrEmpty(errPtypes.ToString()))
                {
                    string tmpMsg = i >= 5 ? errPtypes.ToString().Trim(new char[] { ';', '\r', '\n' }) + " 等;\r\n" : errPtypes.ToString();
                    response.IsSuccess = false;
                    response.ErrorMsg = "\r\n以下商品存在负库存：\r\n" + tmpMsg;
                    return response;
                }
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
            //修改订单
            //dbHelper.BeginTransaction();
            //var result = AllotOrderSqlManager.SaveAllotOrder(dbHelper, request.ProfileId, request.SaveEntity);
            //if (result)
            //{
            response.IsSuccess = true;
            response.ErrorMsg = null;
            //    dbHelper.CommitTransaction();
            //}
            //else
            //{
            //    dbHelper.RollbackTransaction();
            //}


        }
        catch (Exception ex)
        {
            throw new MessageException(string.Format("保存调拨计划单出错，错误：{0}", ex.Message));
        }
    }
    return response;
}
