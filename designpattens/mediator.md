中介者模式用来封装一组对象的交互。以此让这些对象从本来的 紧密耦合到松耦合，让这些对象可以各自独立变化。

在一个GUI应用中,  WinForm ,WebForm，ViewController都是一种Mediator，让内部的控件的事件处理和控件访问都在一起。实际上，每个Form都会有很多buttons, text ，list boxe等等。如果它们之间直接发消息的话，大家都像是蜘蛛网一样的耦合在一起。实际上，大家都是通过Form来完成通讯的。

比如一个应用有一个数字显示和一个按钮，当点击按钮时，设置数字加一。
<div id="app">
<p><span id="count">1</span>
    <button id="inc">+</button>
    <button id="dec">-</button>
  </p>
</div>
那么，耦合起来的方案是这样的(伪代码)：

    class Button1 {
        constructor(text){this.textbox = textbox}
        click(){
            this.textbox.value += 1
        }
    }
    class Form1 {}
    class Textbox1 {
    }

真实的做法一般是这样的(伪代码)：

    class Button1 {
    }
    class Form1 {
        constructor(){
            this.textbox = new Textbox1()
            this.button = new Button()
            this.button.click = this.click
        }
        click(){
            this.textbox.value +=1
        }
    }
    class Textbox1 {
    }
    
当各种控件增加，越来越多时，后一种中介者模式就体现出很大的好处来。至少，其中的各种控件直接即使需要消息通讯，也不必比如引用，而只要和中介者Form1耦合即可。
