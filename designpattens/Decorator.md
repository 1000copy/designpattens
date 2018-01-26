Decorator模式可以让用户不必修改现存对象，而是新加入一个对象，以此新对象包装现存对象，在新对象上增加额外功能，新的功能函数和老的功能接口函数一致。

案例中为了可以使用颜色绘制现在的Shape，做法是添加新类，包装老对象，在新类中添加颜色绘制功能。

![](https://user-gold-cdn.xitu.io/2018/1/2/160b61d39db1d8fb?w=583&h=373&f=jpeg&s=23973)

代码：

    public class DecoratorPatternDemo {
       public static void main(String[] args) {
          Shape circle = new Circle();
          Shape redCircle = new RedShapeDecorator(new Circle());
          Shape redRectangle = new RedShapeDecorator(new Rectangle());
          System.out.println("Circle with normal border");
          circle.draw();
          System.out.println("\nCircle of red border");
          redCircle.draw();
          System.out.println("\nRectangle of red border");
          redRectangle.draw();
       }
    }