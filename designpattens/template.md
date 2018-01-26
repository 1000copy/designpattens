一个操作的具体步骤是确定的，但是它的每个具体步骤的实现各不相同。此时可以使用模板模式。

比如要做一个模块，它可以打印带有表头表体表尾的数据到HTML或者是Console:

1. 基础类定义执行步骤，打印表头、表体、表尾
2. 派生类精细化或者调整具体步骤的实现。在自己的特定的表头等打印步骤中为自己的类实现特定内容。比如HTMLReport应该为数据设置上标签。

UML图是这样的：


![](https://user-gold-cdn.xitu.io/2018/1/12/160e99a3f526a0cd?w=332&h=304&f=png&s=12641)

模板方法如下：

    public abstract class Report {
       //模板方法
       public final void play(){
          printHeader();
          printDetail();
          printFooter();
       }
    }
    
如下案例，在Game基类也定义了一系列的步骤，然后由不同的子类细化或者定义它们的实际操作：

![](https://user-gold-cdn.xitu.io/2018/1/3/160baeb83c31aad1?w=560&h=372&f=jpeg&s=19341)

基类模板方法如下：

    public abstract class Game {
       abstract void initialize();
       abstract void startPlay();
       abstract void endPlay();
       //模板方法
       public final void play(){
          initialize();
          startPlay();
          endPlay();
       }
    }

