# 对重构报告的要求

为了做到更好的软件产品内在质量，以及积累更加容易复用的重构方法，培养一批“写得一手好代码的程序员”，指定重构报告应该包含的内容如下：


1. 指出当前的代码逻辑上下文（可选）
2. 指出完成的方向性效果。比如完成后代码块职责变得单一，嵌套深度减少，动名词结构变得简洁，过程化变得面向对象化等
3. 要列出具体的坏味道。列出深入探查中发现的具体问题，比如违反单一职责原则，违反嵌套嵌套深度。这些“违反”，其实就是重构提到的“坏味道”，bad smell，比如数据泥团，散弹枪等，因为这样做的话，发现问题的过程就是可以复用的
4. 提出修改的方法，列出重构的手法，比如guard clause，replace type code with strategy，因为这样做的话，手法就是可以复用的

格式的话，要求使用markdown。最好的格式化代码类型报告的工具。当然，实际上，有很多不好的方式，比如：

1. 不做格式化，直接在邮件内贴入代码，真是乱的一逼
2. 直接贴图。无法拷贝代码到自己的编辑器内查看，很不方便

关于协作方面，就是这样的原则：谁重构的，谁发报告，直接给我，抄送团队leader。

多说一句：

我猜想的话，大家可能需要认真学习下《重构》一书中的“坏味道”一章，以及各种重构的手法。如果靠着对重构的字面理解去做重构，那不是一个专业技术人员可以有的行为。

[进展报告](progress.md)

