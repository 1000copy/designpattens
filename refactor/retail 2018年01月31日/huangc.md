markdown格式正文

### 代码质量优化报告
1. 完成方向：之前代码重复率比较高,完成后简化代码重复率减小。
2. 代码坏味道：重复代码
3. 修改方法：以函数提炼重复代码
#### 修改前代码

```
public void GetNoticesCount(object noticeEntity)
        {
            NoticesOrderCountEntity entity = new NoticesOrderCountEntity();
            if (noticeEntity != null)
            {
                entity = JsonConvert.DeserializeObject<NoticesOrderCountEntity>(noticeEntity.ToString());
            }
            var orderNoticesCount = entity.NoticeOrderCount + entity.InOutOrderCount + entity.AllotOrderCount;

            if (entity.NoticeOrderCount > 9)
            {
                OrderBg = "../Images/10.png";
            }
            else
            {
                OrderBg = string.Format("../Images/{0}.png", entity.NoticeOrderCount);
            }

            if (entity.InOutOrderCount > 9)
            {
                InOutOrderBg = "../Images/10.png";
            }
            else
            {
                InOutOrderBg = string.Format("../Images/{0}.png", entity.InOutOrderCount);
            }

            if (entity.AllotOrderCount > 9)
            {
                AllotOrderBg = "../Images/10.png";
            }
            else
            {
                AllotOrderBg = string.Format("../Images/{0}.png", entity.AllotOrderCount);
            }

            if (orderNoticesCount > 9)
            {
                NoticesCountBg = "../Images/10.png";
            }
            else
            {
                NoticesCountBg = string.Format("../Images/{0}.png", orderNoticesCount);
            }
           
        }
```
#### 修改后代码

```
        public void GetNoticesCount(object noticeEntity)
        {
            NoticesOrderCountEntity entity = new NoticesOrderCountEntity();
            if (noticeEntity != null)
            {
                entity = JsonConvert.DeserializeObject<NoticesOrderCountEntity>(noticeEntity.ToString());
            }
            var orderNoticesCount = entity.NoticeOrderCount + entity.InOutOrderCount + entity.AllotOrderCount;

            OrderBg = SetBackGround(entity.NoticeOrderCount);
            InOutOrderBg = SetBackGround(entity.InOutOrderCount);
            AllotOrderBg = SetBackGround(entity.AllotOrderCount);
            NoticesCountBg = SetBackGround(orderNoticesCount);
        }
        
        private string SetBackGround(int count)
        {
            if (count > 9)
            {
                return "../Images/10.png";
            }
            else
            {
                return string.Format("../Images/{0}.png", count);
            }
        }
```
