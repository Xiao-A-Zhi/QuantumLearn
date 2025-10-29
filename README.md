# QuantumLearn

一个基于 Photon Quantum 的学习项目，展示如何使用确定性 ECS 框架开发多人在线游戏。

## 📖 项目简介

QuantumLearn 是一个 Unity 学习项目，使用 Photon Quantum v3 框架构建。项目包含了一个经典的 Asteroids（小行星）游戏 demo，展示了如何使用 Quantum 的确定性物理引擎和 ECS（Entity Component System）架构来开发高性能的多人在线游戏。

### 关于 Photon Quantum

Photon Quantum 是一个高性能的确定性 ECS 框架，专为使用 Unity 开发的多人在线游戏而设计。它采用预测/回滚（predict/rollback）方法，非常适合对延迟敏感的在线游戏，如动作 RPG、体育游戏、格斗游戏、FPS 等。

**核心特性：**
- 服务器管理的预测/回滚模拟核心
- 稀疏集（Sparse-set）ECS 内存模型和 API
- 完整的无状态确定性库（数学、2D 和 3D 物理、导航等）
- 丰富的 Unity 编辑器集成和工具

## 🚀 快速开始

### 环境要求

- Unity 2022.3.62f2 或更高版本
- Photon Quantum SDK v3.0.8
- 支持 .NET Standard 2.1

### 安装步骤

1. **克隆仓库**
   ```bash
   git clone https://github.com/Xiao-A-Zhi/QuantumLearn.git
   cd QuantumLearn
   ```

2. **使用 Unity Hub 打开项目**
   - 打开 Unity Hub
   - 点击 "Add" 添加项目
   - 选择项目根目录
   - 使用 Unity 2022.3.62f2 或兼容版本打开

3. **运行游戏**
   - 打开场景：`Assets/Scenes/AsteroidsGameplay.unity`
   - 点击 Play 按钮运行游戏

### 控制方式

- **↑（上方向键）**：前进
- **←（左方向键）**：向左旋转
- **→（右方向键）**：向右旋转
- **Fire 键**：发射（待实现）

## 📁 项目结构

```
QuantumLearn/
├── Assets/
│   ├── Photon/                    # Photon SDK 文件
│   │   ├── PhotonLibs/           # Photon 库
│   │   ├── PhotonRealtime/       # Photon Realtime 传输层
│   │   └── Quantum/              # Quantum SDK
│   │       ├── Assemblies/       # Quantum 核心库 (netstandard2.1)
│   │       ├── Editor/           # 编辑器脚本
│   │       ├── Runtime/          # Quantum Unity 运行时
│   │       └── Simulation/       # Quantum 模拟代码
│   ├── QuantumUser/              # 用户工作空间（游戏逻辑）
│   │   ├── Editor/               # 编辑器扩展
│   │   │   ├── CodeGen/         # CodeGen 设置
│   │   │   └── Generated/       # 生成的编辑器脚本
│   │   ├── Resources/            # Quantum 配置文件
│   │   ├── Scenes/               # 游戏场景
│   │   ├── Simulation/           # 确定性模拟代码
│   │   │   ├── Generated/       # Qtn 文件生成的代码
│   │   │   ├── Input.qtn        # 输入定义
│   │   │   └── AsteroidsShipSystem.cs  # 飞船系统
│   │   └── View/                 # Unity 视图脚本
│   │       └── Generated/       # 生成的资产脚本
│   ├── Resources/                # Unity 资源
│   └── Scenes/                   # 场景文件
│       ├── AsteroidsGameplay.unity  # Asteroids 游戏场景
│       └── SampleScene.unity        # 示例场景
├── Packages/                      # Unity 包管理
├── ProjectSettings/               # Unity 项目设置
└── README.md                      # 项目说明文档
```

### 关键目录说明

- **Assets/QuantumUser/Simulation/**：所有确定性游戏逻辑必须放在这里
  - 使用确定性的数学库（FP、FPMath）
  - 避免使用非确定性的 Unity API
  - 代码会被编译到 `Quantum.Simulation.dll`

- **Assets/QuantumUser/View/**：Unity 视图层代码
  - 负责游戏的表现层
  - 可以使用 Unity API
  - 与确定性模拟代码解耦

- **Assets/QuantumUser/Simulation/Generated/**：自动生成的代码
  - 由 Quantum CodeGen 根据 `.qtn` 文件生成
  - 不要手动修改这些文件

## 🎮 已实现功能

### Asteroids 游戏系统

- [x] **飞船移动系统** (`AsteroidsShipSystem.cs`)
  - 前进加速
  - 左右旋转控制
  - 物理驱动的移动
  - 角速度限制

- [x] **输入系统** (`Input.qtn`)
  - 方向键输入
  - 射击按钮（待实现逻辑）

- [x] **确定性物理**
  - 使用 Quantum 的 2D 物理引擎
  - Transform2D 和 PhysicsBody2D 组件
  - 力和扭矩控制

## 🛠️ 技术栈

### 核心框架
- **Unity 2022.3.62f2**：游戏引擎
- **Photon Quantum v3**：确定性 ECS 框架
- **Photon Realtime**：网络传输层

### 架构模式
- **ECS（Entity Component System）**：数据驱动的架构
- **确定性模拟**：保证多人游戏的一致性
- **预测/回滚**：减少网络延迟影响

### 使用的库
- **Quantum.Deterministic**：确定性数学和物理
- **Quantum.Engine**：核心引擎功能
- 2D Physics、Navigation 等完整的确定性库

## 📚 学习资源

### 官方文档
- [Photon Quantum 官方文档](https://doc.photonengine.com/quantum/v3)
- [Quantum ECS 指南](https://doc.photonengine.com/quantum/v3/getting-started/ecs)
- [确定性物理](https://doc.photonengine.com/quantum/v3/manual/physics)

### 代码示例
- 查看 `AsteroidsShipSystem.cs` 了解系统的编写方式
- 查看 `Input.qtn` 了解输入定义语法
- 参考 `Assets/Photon/Quantum/README.md` 了解 SDK 结构

## 🔧 开发指南

### 添加新系统

1. 在 `Assets/QuantumUser/Simulation/` 创建新的 `.cs` 文件
2. 继承自 `SystemMainThreadFilter<T>` 或其他系统基类
3. 定义 Filter 结构体声明所需的组件
4. 实现 `Update` 方法编写游戏逻辑

```csharp
namespace Quantum.QuantumUser.Simulation
{
    public unsafe class MySystem : SystemMainThreadFilter<MySystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            // 游戏逻辑
        }
    }
}
```

### 定义输入

在 `.qtn` 文件中定义输入（参考 `Input.qtn`）：

```qtn
input {
    button Left;
    button Right;
    button Up;
    button Fire;
}
```

### CodeGen 生成代码

修改 `.qtn` 文件后，Unity 会自动运行 CodeGen 生成相应的 C# 代码。生成的代码位于 `Generated` 文件夹中。

## 🚧 待实现功能

- [ ] 小行星生成系统
- [ ] 射击系统
- [ ] 碰撞检测和销毁
- [ ] 分数系统
- [ ] 多人联机支持
- [ ] UI 界面
- [ ] 音效和特效

## 📝 注意事项

1. **确定性代码规则**
   - 使用 `FP` 代替 `float`
   - 使用 `FPVector2/3` 代替 `Vector2/3`
   - 避免使用 `Random.Range()`，使用 `frame.RNG`
   - 不要在 Simulation 代码中使用 Unity API

2. **版本控制**
   - `Assets/QuantumUser/` 内容应纳入版本控制
   - `Assets/Photon/` 内容会在升级时被替换

3. **调试技巧**
   - 使用 Quantum 的内置调试工具
   - 利用确定性特性进行重放调试

## 📄 许可证

本项目基于 Photon Quantum SDK 构建，遵循 Photon Engine 的许可协议。详见 [Photon Engine 许可条款](https://www.photonengine.com/terms)。

## 🙏 致谢

- [Photon Engine](https://www.photonengine.com/) - 提供优秀的多人游戏网络解决方案
- Unity Technologies - 提供强大的游戏开发引擎

## 📧 联系方式

如有问题或建议，欢迎提交 Issue 或 Pull Request。

---

**Happy Learning! 🎓**
