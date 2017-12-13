# designpattens - factories

作为创建型设计模式，带有工厂名字的设计模式共有三个，分别是

1. Simple Factory
2. Factory Method
3. Abstract Factory

其中的 Simple Factory并不是GoF一书中的模式，但是它是最基础最常用的，并且也是循序渐进的了解另外两个工厂的必要基础，所有放在一起讲它们是比较科学的。

三者常常是容易搞混的，我就见过若干个搞混的案例。要么，比起这更难的，是不太容易弄明白使用的场景和目的。本文试图通过一个案例，讲清楚三者的内涵，但是不准备讲解它的外延。

假设我们想要做一个图形编辑器，我们把它的需求压低到极为简洁的形式，只要和当前要描述的问题无关的，我们都不会引入：

1. 可以创建两种形状，矩形和圆形
2. 可以设置形状的颜色，红色和黄色

那么，系统中必然需要如下的Shape类：
    class Shape{draw(){}}
    class Rect extends Shape{draw(){}}
    class Circle extends Shape{draw(){}}
以及，系统中必然需要如下的Color类：
    class Color{fill(){}}
    class Red extends Color{fill(){}}
    class Yellow extends Color{fill(){}}
我们首先从Shape开始。假设需要创建一个矩形，我们可以这样做：

    var rect = new Rect()

需要一个圆形也简单：

    var rect = new Circle()

实际上，我们通常是在界面上，一般是工具栏上，放置两个按钮，让用户选择哪个按钮，然后创建此形状。用户选择了矩形，接下来创建就是矩形，选择的是圆形，那么创建就是圆形。所以这样的代码一定是存在的：

    if (userSelected = "rect")
      return new Rect()
    if (userSelected = "circle")
      return new Circle()
## Simple Factory

 Simple Factory的价值就是让调用者从创建逻辑中解脱，只要传递一个参数，就可以获得创建对象。实际上，从对象职责来说，这段代码不应该是Rect或者是Circle的，也不应该是UI类的，UI类在不同的应用中是不一样的，但是我们知道作为顶层类，需要负责UI显示和事件，不应该负责创建对象的逻辑。实际上，很多代码放到此处，特别容易导致代码拥挤，主控类职责过多的问题。

最好引入一个新的类，像是这样：
    class ShapeCREATEOR{
        create(userSelected){
            if (userSelected = "rect")
              return new Rect()
            if (userSelected = "circle")
              return new Circle()
        }
    }
这个类的所有逻辑，都是专门用于创建其他类。因为非常常见，人们为他取名为Factory，其他被创建的类被称为Product。所以惯例上来说，此类的名字会冠以工厂名：
    class ShapeFactory
根据传入的参数，决定创建哪一个产品类，此类就被称为简单工厂类（Simple Factory）。有了工厂类，使用者就可以直接使用工厂类获得需要的对象：
    var sf = new ShapeFactory()
    var rect = sf.create("rect")

于是，所有需要创建矩形的场合，你知道，一个UI App，除了工具栏，还有菜单，都只要写这样的代码就可以创建了。而不必到处根据userSelected来做分支了。这就是使用工厂的好处。如果支持命令创建，甚至使用json文件中恢复对象时，本来也需要传递字符串来决定创建对象时，就显得简单工厂的好处了。


## factory method

简单工厂根据传入的参数决定实例化哪一个类，而factory method有子类来决定实例化哪一个类。

    class Shape{draw(){}}
    class Rect extends Shape{draw(){}}
    class Circle extends Shape{draw(){}}
    class ShapeFactory{
    	createShape(){}
    }
    class RectFactory extends ShapeFactory{
    	createShape(){return new Rect()}
    }
    class CircleFactory extends ShapeFactory{
    	createShape(){return new Circle()}
    }

调用者需要创建Rect，只要这样：

      var f = new RectFactory()
      f.createShape()

这是factory method的定义：

     创建一个接口，但是由子类决定实例化哪一个类
     
这里提到的接口是ShapeFactory.createShape,提到的子类为：RectFactory，CircleFactory。这样做就意味着，在工厂内不必根据传入参数分支，它作为子类本身就知道要创建的是哪一个产品。使用对应的工厂，创建需要的类。


# AbstractFactory


要是我们创建的类型不仅仅是Shape，还有Color的话，AbstractFactory就有价值。AbstractFactory提供一个接口a，此接口可以创建一系列相关或者相互依赖的对象b，使用用户不需要指定具体的类即可创建它们c。

我们先看代码：

    class Shape{draw(){}}
    class Rect extends Shape{draw(){}}
    class Circle extends Shape{draw(){}}
    class ShapeFactory{
    	createShape(type){
    		if (shape == "rect"){
    			return new Rect()
    		}else{
    			return new Circle()
    		}
    	}
    }
    class Color{fill(){}}
    class Red extends Color{fill(){}}
    class Yellow extends Color{fill(){}}
    class ColorFactory {
    	creatColor(type){
    		if (shape == "Red"){
    			return new Red()
    		}else if (shape == "Yellow"{
    			return new Yellow()
    		}
    	}
    }

如果希望客户可以一个单一接口来访问Color和Shape，可以引入一个抽象工厂：

    class AbstractFactory{
    	createShape(){}
    	createColor(){}
    }

要求两个工厂实现此抽象工厂：

    class ShapeFactory extends AbstractFactory{
    	createShape(type){
    		if (shape == "rect"){
    			return new Rect()
    		}else{
    			return new Circle()
    		}
    	}
    	createColor(){
    		return null
    	}
    }
    
    class ColorFactory extends AbstractFactory{
    	createShape(type){return null}
    	creatColor(type){
    		if (shape == "Red"){
    			return new Red()
    		}else if (shape == "Yellow"{
    			return new Yellow()
    		}
    	}
    }
自己不具备的能力，不实现即可，这里就是返回一个null。需要一个创建工程的简单工厂
    class FactoryProducer{
    	getFactory(type){
    		if (type == "color")return new ColorFactory()
    			else return new ShapeFactory()
    	}
    }

没有抽象工厂，那么代码是这样的，所有的Factory类的创建都是硬编码的

    var sf = new ShapeFactory()
    var r = sf.createColor("Rect")
    r.draw()
    var cf = new ColorFactory()
    var c = cf.createColor("Red")
    c.fill()

有了抽象工厂，那么客户的使用就是这样

    var fp = new FactoryProducer()
    var sf = fp.getFactory("shape")
    var r = sf.createColor("Rect")
    r.draw()
    var cf = fp.getFactory("color")
    var c = cf.createColor("Red")
    c.fill()

好处是，硬编码创建的类只有一个，就是FactoryProducer。

其中难懂的部分，做一个进一步说明：

1. 接口a：AbstractFactory内的两个函数createShape，createColor
2. 一系列相关或者相互依赖的对象b: Shape系列类，Color系列类
3. 使用用户不需要指定具体的类即可创建它们c:实际上，用户只要使用FactoryProducer这一个类，不需要使用任何一个工厂，以及工厂创建的类。

参考：

1. 以shapeEditor为例，讲述Shape创建的过程。Shape ,Color是需要创建的一组对象。Shape包括rect,line等。color包括red，blue等。Shape和Color两个一组，构成了“一个系列的，互相相关的一组对象或者接口”，如GoF所说的。Shape的创建可以通过ShapeFactory，它是一个简单工厂，Color的创建则是通过ColorFactory，也是一简单工厂。AbstractFactory则提供了两个简单工厂的共性的方法，提供给两个简单工厂继承或者实现。FactoryProducer可以根据传入参数创建一个AbstractFactory的对象，这个对象可能是两个简单工厂对象之一。所以，文中提到抽象工厂其实可以被称为工厂类的工厂类。这大概是我见过的最简明的abstract factory了：This factory is also called as factory of factories. This type of design pattern comes under creational pattern as this pattern provides one of the best ways to create an object. https://www.tutorialspoint.com/design_pattern/abstract_factory_pattern.htm
2. 再来一个木门和木匠，铁门和铁匠，两个工厂，木工厂，铁工厂，和抽象两者的共同工厂。https://github.com/sohamkamani/javascript-design-patterns-for-humans
3. 这个案例，要创建的对象是书籍和发布渠道，书籍包括诗歌、论文，发布渠道包括Blog和科技期刊。是的，构成了“一个系列的，互相相关的一组对象或者接口”，如GoF所说的。尽管PoemFactory和PublisherFactory都有了，但是这个案例并没有一个AbstractFactory作为两者的共同基类，因此和经典的抽象工厂模式还是不完全一致的，它没有FactoryProducer和AbstractFactory，而是直接在main()函数内创建两者。这一差别是值得注意的。还过得去的abstract factory介绍 https://airbrake.io/blog/design-patterns/abstract-factory
4. what is factory method - A factory function is any function which is not a class or constructor that returns a (presumably new) object. In JavaScript, any function can return an object. When it does so without the new keyword, it’s a factory function. 重构和模式一书也提到了factory method的概念是比较模糊的，不如叫做create method 
5. what is simple factory。尽管标题是factory method ，其实是一个Simple factory ,因为它并没有让子类来决定去实例化哪一个类。  “Define an interface for creating an object, but let the subclasses decide which class to instantiate. The Factory method lets a class defer instantiation to subclasses”http://www.dofactory.com/javascript/factory-method-design-pattern 但是此案例好在，它支持了标准的模式中的术语，在js中的使用情况，比如abstract product并不需要在js出现。因为js没有接口或者虚拟类，也不需要。
6. 重构和模式一书提到： 当一个类内的构造函数比较多以至于让类的职责看起来不明显时，有必要分离这些构造过程到另外一个对象，这个新的对象负责前一个对象的构造工作。他举出来的案例，贷款对象就是很多的构造函数，分离构造出来是非常必要的。进一步阅读:Why should I use a factory class instead of direct object construction?
7. 这里的系列产品，包括玩具车，玩具直升机，它们都有对应的工厂，再加上抽象工厂。 https://www.binpress.com/tutorial/the-factory-design-pattern-explained-by-example/142
8. 简单工厂和工程方法的案例，使用支付方法。很不错。	 https://stackoverflow.com/a/44105480/3781367

