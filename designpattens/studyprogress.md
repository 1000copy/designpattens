## 目录



## 学习方法和体会：设计模式
![alt](img.png)
我在之前几周做tdd和重构的培训时，我会发现不管我讲什么，在讨论环节都会有人提到设计模式。曾经有人说，”每一个程序员的案头都会有一本设计模式的书“。程序员对模式的着迷是显而易见的。至于着迷的原因，我们也就不去深究了，因为我搞不懂。

这是一个分享，分享我这期间研读设计模式的体会。

1. 一本书为主，几本书对照看
2. 充分利用google和Stack Overflow和Wikipedia，对同一个概念做横向的研究和探讨
3. 找到对应的代码，最好是能够运行的。没有代码，都是胡闹。


互联网让研究一个问题，变得比起以往简单太多了。

没什么耐心了解过程，想要直奔主题的，看这里：

	https://segmentfault.com/a/1190000012414143 

本文讲的是研究的过程。适合想要做研究的人阅读。我从“设计模式"这本书开始的，英文的中文的都看过，然而，看懂它不太容易，因为案例的分散的，其中有lexi编辑器，有迷宫等等。“重构和模式”一书的案例中一个是贷款的，更加不易理解，当然它提到的代码的构造过程的参数，对于一个不懂业务的人来说，构造一个对象太难了。这一点对我来说印象深刻。一下就理解了工厂模式解决的痛点。理解案例的特定领域本身不太容易，和自己的工作相关性也不太高。

接下来也看了head first design patten，还有也看了”大话设计模式“，后面这两本书是看懂了的，但是看完了，也就拉倒了。比如对应工厂方法，三个类型的工厂，其实不太知道用到怎么样的场合。简单工程当然没有问题，工厂方法应该是用过，抽象工厂肯定是没有用过的。

这次又想要研究，就继续从"设计模式"研读，我决定真正搞懂它们，但是，我不在考虑正常的一次一个的把23个设计模式以纵向的过完，而是横向的去研究，比如说，abstract factory，我一次研究透彻，通过google，找到wikiapedia的词条，Stack Overflow上的讨论，一次系统的把它们研究透。实际上我发现了几个不错的站点：

1. tutorialspoint 站点： https://www.tutorialspoint.com/design_pattern/

这是一个专门写教程的站点，很多教程我之前都通过google查找看到过，质量很高。特点是简洁，没有废话。在design patterns中的编写方法，一般就是定义总括，UML图，代码和输出结果。我专门反复的看了其中的AbstractFactory，发现确实简明的够可以，也了解到这个模式的特定之处。老实说，看到AbstractFactory的定义是factory of factory，我一下明白了不少疑问。简单工程创建产品，而抽象工厂把工厂当成产品来创建，因此是工厂的工厂！

2. wikipedia  站点。 https://en.wikipedia.org/wiki/Abstract_factory_pattern


这个站点的论述：The abstract factory pattern provides a way to encapsulate a group of individual factories that have a common theme without specifying their concrete classes，这个定义是和tutorialspoint不同的，它定义的更加全面完整。是一个很不错的角度。后面跟着的各种代码值得一看。

3. 这个博客让我加深了对创建”系列化产品“的认识。因为更多案不同的场景的案例，就可以帮助透彻理解概念的应用。

 	https://airbrake.io/blog/design-patterns/abstract-factory

 这里要创建的系列产品变成了。书籍和发布渠道，书籍包括诗歌、论文，发布渠道包括Blog和科技期刊。是的，构成了“一个系列的，互相相关的一组对象或者接口”，如GoF所说的。尽管PoemFactory和PublisherFactory都有了，但是这个案例并没有一个AbstractFactory作为两者的共同基类，因此和经典的抽象工厂模式还是不完全一致的，它没有FactoryProducer和AbstractFactory，而是直接在main()函数内创建两者。这一差别是值得注意的。还过得去的abstract factory介绍 

4. Stack Overflow上面的大量讨论。对于概念的理解非常很有价值，提供了更多的类型的案例。可以通过

- what is the purpose of abstract factory ? 
- what is  abstract factory?
- why use abstract factory?
- when use abstract factory?

找到代码很重要。我看了设计模式，总觉得少点什么，那个第一章的lexi编辑器，难道不是应该有代码吗。没有代码，这么空着说。果然，代码是有的。查下“design patterns lexi source code insite:stackoverflow.com”，发现：

	The Gof tell us in a note that Lexi is based on "Doc, a text editing application developed by Calder". But this paper only outlines an editor, without any source. And I even believe today that Lexi never truly existed as a program.

既然是光盘，就算了。但是网上确实有好事者自己做了一个。

	https://github.com/AmitDutta/lexi

另外，既然很多人喜欢用Shape做案例，是否有人写一个shapeEditor？有的。查这个：shape editor js 。这个代码案例给我很深的印象，它里面就有一个工厂。并且对于“创建一个对象的过程太过复杂”这句设计模式常常提到的需要使用factory模式的话，我也立即get到了。因为代码中创建一个rect的过程就是复杂的，需要处理数据的down，drag和up并做计算，动态显示rect等，得有即使行代码呢。


几天的大量研读，积累了很多资料，随后我决定把它们写出来，我大概花了几个小时的时间，完成了这篇文章。

一个案例，讲清楚三个设计模式。