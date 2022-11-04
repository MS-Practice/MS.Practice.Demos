# 强类型Id

在业务开发中，尤其是在领域驱动开发中经常遇见，以充血模型为代表；某些字段具有丰富的业务意义：有格式校验、逻辑校验以及一些单独的业务规则等。

这个时候如果还是用类型原语来解决，那么就难以避免出现了散落在各处的代码逻辑。虽然我们可以通过封装到一个单独的帮助类来避免这种情况。但是这就带来了大量没必要的方法调用。

这个时候我们可以通过将某些具有特别意义字段（如订单Id，用户地址等）进行强类型化做处理，所有的逻辑就可以像单独的帮助类一样聚合到一起，并也能像正常属性一样访问。

为了达到这种效果，要做非常多的准备工作，包括序列化与反序列化，类型转换等。

这种技术也叫 **Primitive Obsession**

这个 Demo 就是处理类似问题的样本代码。

下面的资料强烈建议阅读；

## 参考资料

- https://andrewlock.net/series/using-strongly-typed-entity-ids-to-avoid-primitive-obsession/
- https://lostechies.com/jimmybogard/2007/12/03/dealing-with-primitive-obsession/
- https://blog.stephencleary.com/2022/10/modern-csharp-techniques-2-value-records.html