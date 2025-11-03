# 概念速查（本项目如何使用 Quantum）

为避免在代码里反复解释基础概念，以下集中说明。代码注释以系统/配置的“行为意图”为主。

- Preserve（防裁剪）
  - Unity IL2CPP/Linker 可能会移除看似未被直接引用的类型。给类加 [Preserve] 可避免被裁剪，便于反射/生成/信号系统正常工作。

- unsafe 与指针
  - 在性能敏感路径中，Quantum 提供 Unsafe API（如 TryGetPointer/GetPointer）返回组件的原始指针，零拷贝读写。
  - 指针仅在“当前帧”有效，不可跨帧/跨线程保存或使用。

- Frame（帧上下文）
  - 每个系统方法都收一个 Frame 参数，代表当前确定性模拟帧的“操作表面”。
  - 通过它进行：实体/组件读写、创建销毁、信号发送、资源查找、确定性 RNG 等。
  - 生命周期与确定性都以帧为边界：所有读取与写入在该 tick 内完成。

- 过滤器与系统（SystemMainThreadFilter<T> / SystemSignalsOnly）
  - 过滤器系统：声明 Filter 结构体，列出要处理的组件指针，系统在主线程遍历匹配实体并调用 Update。
  - 信号系统：仅处理事件（如 OnCollisionEnter2D、玩家加入、开火事件），不做逐实体循环。

- TryGetPointer / GetPointer
  - TryGetPointer<T>(entity, out T*): O(1) 判断并取组件指针；不存在返回 false。
  - GetPointer<T>(entity): 假定存在并直接返回指针；若不存在会抛错（避免在热路径反复判空）。

- 为什么是 O(1)：Sparse-set（稀疏集）
  - 组件按稀疏/稠密数组存放，通过实体 ID 快速映射到稠密索引，常数步数即可访问到组件数据。缓存友好。

- Transform2D.Up 与物理
  - Transform2D 提供局部坐标系方向单位向量（Up/Right）。常用 Up 作为“朝前”发射或施力方向。
  - PhysicsBody2D 提供 AddForce/AddTorque/Velocity/AngularVelocity 等物理控制。

- 碰撞回调：CollisionInfo2D 的 Entity / Other
  - Entity：从本次回调“本方”视角的实体；Other：碰撞的对方。
  - 不要假设某类对象永远是 Entity 或 Other，实际逻辑应通过组件存在性判断。

- 帧边界与确定性（锁步）
  - 给定相同初始状态 + 每帧相同输入，所有客户端每个帧结束后的状态必须一致。
  - 禁用非确定性来源（float、系统时间、非确定性随机、未管控并发）。使用 FP 与 frame.RNG。

- 确定性随机（frame.RNG）
  - 不使用 UnityEngine.Random。在模拟代码中，统一通过 frame.RNG 获取随机，且 RNG 种子/序列在各端一致。

更多可参考：Photon Quantum 官方文档（ECS/Physics/Determinism/Signals）。
