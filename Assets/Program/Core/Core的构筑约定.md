core里是一组可以互相引用的工具类。

目的是为了嵌在各个项目中可用，

这就要求core一定是gameplay更下层的一层，**即core不需引用core之外的类**，以保证core只需维护一个最小的集合来很容易的迁移到其他项目中