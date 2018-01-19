Composite模式，可以帮助客户代码以一致的方式来操作复合对象和单体对象。

比如说，对一个shape对象设置颜色，和对一组被选中的shape设置颜色时使用的操作是一致的：

    rect.setColor(red)
    circle.setColor(red)
和：

    shapes.add(rect)
    shapes.add(cirle)
    shapes.setColor(red)

具体做法就是一个Component基类提供复合对象和单体对象一致需要提供的操作接口，然后由复合对象和单体对象实现这个操作接口。并令复合对象可以包含Component类型的对象。此模式以树形结构来构建一组对象。一个类可以包含它自己的一组对象。当对一个Composite操作时，对其内涵的对象集合做一个循环，即可以对操作一个对象相似的方式来操作一组对象。

![](https://user-gold-cdn.xitu.io/2018/1/2/160b61442d6b772c?w=600&h=240&f=jpeg&s=56517)

代码：

    interface Graphic {
        //Prints the graphic.
        public void print();
    }
    /** "Composite" */
    class CompositeGraphic implements Graphic {
        //Collection of child graphics.
        private List<Graphic> childGraphics = new ArrayList<Graphic>();
        //Prints the graphic.
        public void print() {
            for (Graphic graphic : childGraphics) {
                graphic.print();
            }
        }
        //Adds the graphic to the composition.
        public void add(Graphic graphic) {
            childGraphics.add(graphic);
        }
        //Removes the graphic from the composition.
        public void remove(Graphic graphic) {
            childGraphics.remove(graphic);
        }
    }
    /** "Leaf" */
    class Ellipse implements Graphic {
        //Prints the graphic.
        public void print() {
            System.out.println("Ellipse");
        }
    }
    /** Client */
    public class Program {
        public static void main(String[] args) {
            //Initialize four ellipses
            Ellipse ellipse1 = new Ellipse();
            Ellipse ellipse2 = new Ellipse();
            Ellipse ellipse3 = new Ellipse();
            Ellipse ellipse4 = new Ellipse();
            //Initialize three composite graphics
            CompositeGraphic graphic = new CompositeGraphic();
            CompositeGraphic graphic1 = new CompositeGraphic();
            CompositeGraphic graphic2 = new CompositeGraphic();
            //Composes the graphics
            graphic1.add(ellipse1);
            graphic1.add(ellipse2);
            graphic1.add(ellipse3);
            graphic2.add(ellipse4);
            graphic.add(graphic1);
            graphic.add(graphic2);
            //Prints the complete graphic (two times the string "Ellipse").
            graphic.print();
        }
    }

一个日常生活中常常遇到的案例是雇员和它的下属：

![](https://user-gold-cdn.xitu.io/2018/1/2/160b6125b4f84520?w=358&h=420&f=jpeg&s=16926)

另一个常见的例子是表达式计算。"2+3X5" 就是一个合法的表达式，它的，其中的2就是一个NumbricOperand，"3X5"就是一个CompositeOperand。无论是复合操作数，还是单体操作数，都支持一个计算的操作。每个CompositeOperand都包括多个NumbricOperand。

![](https://user-gold-cdn.xitu.io/2018/1/19/1610c7c67887c0bc)
