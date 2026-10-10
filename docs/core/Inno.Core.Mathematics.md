# Inno.Core.Mathematics

[上一页：Logging](Inno.Core.Logging.md) · [Core 索引](README.md) · [下一页：Storage](Inno.Core.Collections.md)

Mathematics 提供引擎自有的 float/int 向量、四元数、4×4 矩阵、矩形、颜色与标量工具，并对热点点积/矩阵运算使用平台 SIMD。多数类型是可变 struct，公开分量字段采用 `x/y/z/w` 或 `m11...m44`。

## 坐标与矩阵约定

`Matrix` 使用 column-vector 语义：`v' = M * v`，`A * B` 表示先应用 B 再应用 A。内存字段按 row-major 命名，平移工厂写入最后一列 `m14/m24/m34`。

矩阵类型本身不绑定 handedness：

- `CreateLookAt` / `CreatePerspectiveFieldOfView`：left-handed。
- `CreateLookAtRH` / `CreatePerspectiveFieldOfViewRH`：right-handed。

```csharp
Matrix world = Matrix.CreateTranslation(position) *
               Matrix.CreateFromQuaternion(rotation) *
               Matrix.CreateScale(scale);
Vector3 worldPoint = Vector3.Transform(localPoint, world);
```

## MathHelper

| API | 说明 |
| --- | --- |
| `C_TOLERANCE` | 默认 float 相对容差 `1e-6f`。 |
| `AlmostEquals(a,b,tolerance)` | 相对容差比较。 |
| `Barycentric(...)` | 重心插值。 |
| `CatmullRom(...)` | Catmull–Rom spline 插值。 |
| `Distance(a,b)` | 标量绝对距离。 |
| `Hermite(...)` | Hermite 插值。 |
| `Lerp` / `LerpUnclamped` / `LerpPrecise` | 线性插值变体。 |
| `SmoothStep(...)` | 平滑插值。 |
| `ToDegrees` / `ToRadians` | 角度单位转换。 |
| `Clamp` / `Saturate` | 范围限制；Saturate 为 0..1。 |
| `IsFinite` | 排除 NaN 与 Infinity。 |
| `WrapAngle` | 将弧度归一到标准周期。 |
| `IsPowerOfTwo(int)` | 正整数 2 的幂检测。 |

## Vector2 与 Vector3

### Vector2

字段 `x/y`；常量 `ZERO`、`ONE`、`UNIT_X`、`UNIT_Y`。

- 长度：`Length()`、`LengthSquared()`、`normalized`、`NormalizeSafe(value, epsilon)`。
- 几何：`Dot`、`Angle`、`SignedAngle`、`Project`、`Reflect`。
- 插值/边界：`Lerp`、`Min`、`Max`。
- 变换：`Transform(Vector2, Matrix)`、`Transform(Vector2, Quaternion)`；Quaternion 路径使用完整 XY 投影旋转矩阵，纯 Z 旋转保持向量长度。
- 运算：`+`、`-`、一元 `-`、float `*`/`/`、近似 `==`/`!=`。
- 与 `System.Numerics.Vector2` 隐式互转；实现 `Equals`、`GetHashCode`、`ToString`。

### Vector3

字段 `x/y/z`；常量 `ZERO`、`ONE`、`UP`、`DOWN`、`LEFT`、`RIGHT`、`FORWARD`、`BACK`。

- 长度与安全归一化 API 同 Vector2。
- 几何：`Dot`、`Angle`、`SignedAngle(from,to,axis)`、`Project`、`Cross`、`Distance`、`Reflect`。
- `Lerp`。
- 变换：`Transform(position, Matrix)`、`TransformNormal(normal, Matrix)`、`Transform(value, Quaternion)`。
- 标准算术/比较 operator；与 `System.Numerics.Vector3` 隐式互转。

```csharp
Vector3 direction = Vector3.NormalizeSafe(target - origin);
float facing = Vector3.Dot(transformForward, direction);
Vector3 tangent = Vector3.Cross(Vector3.UP, direction);
```

## Vector4

字段 `x/y/z/w`；常量 `ZERO`、`ONE`、`UNIT_X/Y/Z/W`。

- `Length`、`LengthSquared`、`normalized`、`Dot`。
- `Lerp`、`Reflect`、`Transform(Vector4, Matrix)`、`ProjectToVector3()`。
- 标准算术；另有 `Matrix * Vector4`。
- 近似 equality；与 `System.Numerics.Vector4` 隐式互转。

## 整数向量

`Vector2Int`、`Vector3Int`、`Vector4Int` 分别公开整数 `x/y[/z/w]`：

- 都提供 `ZERO`、`ONE` 与单位/方向常量。
- 都支持 `+`、`-`、一元 `-`、int `*`/`/`、精确 equality、`Equals`、`GetHashCode`、`ToString`。
- 与对应 float vector 进行显式转换。
- `Vector4Int` 额外提供 `Dot`、`Lerp`、`Reflect`、`Transform(Matrix)`。

整数向量的 float → int 转换使用实现中的显式整数转换规则；涉及取整语义时应先在业务层明确处理。

## Quaternion

字段 `x/y/z/w`，单位旋转为 `identity`。

| 分类 | API |
| --- | --- |
| 范数 | `Length`, `LengthSquared`, `normalized`, `Normalize` |
| 基本运算 | `Conjugate`, `Inverse`, quaternion `operator *`, equality |
| 插值 | `Slerp(a,b,t)` |
| 构造 | `CreateFromAxisAngle`, `FromRotationMatrix`, `LookRotation`, `CreateFromYawPitchRoll` |
| Euler XYZ | `ToEulerAnglesXYZ`, `ToEulerAnglesXYZDegrees`, `FromEulerAnglesXYZ`, `FromEulerAnglesXYZDegrees` |
| Euler ZYX | `ToEulerAnglesZYX`, `ToEulerAnglesZYXDegrees`, `FromEulerAnglesZYX`, `FromEulerAnglesZYXDegrees` |
| 转换 | `ToMatrix()`；与 `System.Numerics.Quaternion` 隐式互转 |

`FromEulerAnglesXYZ` 与 `ToEulerAnglesXYZ` 使用同一个 `Rx * Ry * Rz` 约定，复合角度可以稳定往返；`ZYX` API 保持独立且不应与 XYZ 配对混用。

角度版本未带 `Degrees` 时使用弧度。

## Matrix

公开 16 个 float 字段：`m11`–`m14`、`m21`–`m24`、`m31`–`m34`、`m41`–`m44`，并提供 16 参数构造函数与 `identity`。

| 分类 | API |
| --- | --- |
| TRS | `CreateTranslation(float,float,float/Vector3)`, `CreateScale(float/xyz/Vector3)`, `CreateRotationX/Y/Z`, `CreateFromQuaternion` |
| 投影 | `CreatePerspectiveFieldOfView`, `CreatePerspectiveFieldOfViewRH`, `CreateOrthographic`, `CreateOrthographicOffCenter` |
| 相机 | `CreateLookAt`, `CreateLookAtRH` |
| 代数 | `Multiply`, `Determinant`, `Transpose`, `Invert`, `operator *` |
| 分解 | `Decompose(out scale, out rotation, out translation)`, `Extract2DTransform` |
| GPU 数据 | `CopyToColumnMajor(float[], startIndex)`, `ToColumnMajorArray()` |
| 互操作 | 与 `System.Numerics.Matrix4x4` 隐式互转 |
| 值语义 | `==`, `!=`, `Equals`, `GetHashCode`, `ToString` |

`CopyToColumnMajor` 会验证 destination 容量。向图形 API 上传前应根据后端约定选择它，而不是直接假定 struct 字段布局。

## Rect 与 RectInt

两者公开 `x/y/width/height`，派生属性 `left/right/top/bottom/min/max/size/center`。

- `Overlaps(other)`、`Contains(rect)`、`Contains(point components/vector)`。
- `FromMinMax(min,max)`、`Union(a,b)`、`TryIntersect(a,b,out intersection)`。
- `+`/`-` 按分量运算，支持 equality/value methods。
- `Rect` 与 `System.Numerics.Vector4` 隐式互转。
- `RectInt` 与 `System.Drawing.Rectangle`、`Vector4Int` 隐式互转。

## Color

字段 `r/g/b/a` 使用 0..1 float。构造 `Color(r,g,b,a=1)`；`FromBytes` 和 `ToBytes` 在 byte 通道间转换，`ToUInt32ARGB` 输出 packed ARGB。

内置颜色：`TRANSPARENT`, `WHITE`, `BLACK`, `RED`, `GREEN`, `BLUE`, `YELLOW`, `MAGENTA`, `CYAN`, `GRAY`, `LIGHTGRAY`, `DARKGRAY`, `ORANGE`, `PINK`, `PURPLE`, `BROWN`, `CORNFLOWERBLUE`。

`Color * float` 按通道缩放并 clamp；支持 equality/value methods。

## SimdMath

公开低层 helper：

- `Dot4(Vector128<float>, Vector128<float>)`
- `Dot2(ax,ay,bx,by)`
- `Dot3(ax,ay,az,bx,by,bz)`
- `Dot4(ax,ay,az,aw,bx,by,bz,bw)`

通常应优先使用 Vector API；只有需要避免构造临时向量的底层代码才直接调用这些方法。

## 数值注意事项

- float vector equality 使用容差，而 HashCode 使用原始分量；不要把近似相等的 float vector 依赖为严格 hash-key 等价关系。
- 零向量的 `normalized` 返回 ZERO；Quaternion 近零归一化/求逆返回 identity。
- `Lerp` 是否 clamp 取决于具体 API；需要外插时优先选明确的 Unclamped 版本或先检查实现。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Mathematics.Color`

| 当前声明 | 行为 |
| --- | --- |
| [`(byte R, byte G, byte B, byte A) Inno.Core.Mathematics.Color.ToBytes()`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L88) | Converts normalized color channels to clamped byte values. |
| [`Inno.Core.Mathematics.Color`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L9) | Represents an RGBA color with float components in the range [0, 1]. |
| [`Inno.Core.Mathematics.Color.Color(float r, float g, float b, float a = 1)`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L43) | Creates a validated color instance. |
| [`bool Inno.Core.Mathematics.Color.Equals(Inno.Core.Mathematics.Color other)`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L277) | Determines whether this value and the supplied value represent the same logical state. |
| [`float Inno.Core.Mathematics.Color.a`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L26) | The alpha color component. |
| [`float Inno.Core.Mathematics.Color.b`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L22) | The blue color component. |
| [`float Inno.Core.Mathematics.Color.g`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L18) | The green color component. |
| [`float Inno.Core.Mathematics.Color.r`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L14) | The red color component. |
| [`override bool Inno.Core.Mathematics.Color.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L266) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Color.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L285) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Color.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L121) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.FromBytes(byte r, byte g, byte b, byte a = 255)`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L73) | Creates a normalized color from byte channel values. |
| [`static Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.operator *(Inno.Core.Mathematics.Color c, float factor)`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L204) | Multiplies the supplied values according to their algebraic contract. |
| [`static bool Inno.Core.Mathematics.Color.operator !=(Inno.Core.Mathematics.Color a, Inno.Core.Mathematics.Color b)`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L251) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Color.operator ==(Inno.Core.Mathematics.Color a, Inno.Core.Mathematics.Color b)`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L228) | Determines whether the supplied values are equal under the type's equality tolerance. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.BLACK`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L134) | The black value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.BLUE`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L146) | The blue value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.BROWN`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L186) | The brown value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.CORNFLOWERBLUE`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L190) | The cornflowerblue value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.CYAN`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L158) | The cyan value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.DARKGRAY`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L170) | The darkgray value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.GRAY`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L162) | The gray value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.GREEN`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L142) | The green value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.LIGHTGRAY`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L166) | The lightgray value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.MAGENTA`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L154) | The magenta value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.ORANGE`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L174) | The orange value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.PINK`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L178) | The pink value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.PURPLE`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L182) | The purple value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.RED`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L138) | The red value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.TRANSPARENT`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L126) | The transparent value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.WHITE`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L130) | The white value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Color Inno.Core.Mathematics.Color.YELLOW`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L150) | The yellow value used as part of this type's public representation. |
| [`uint Inno.Core.Mathematics.Color.ToUInt32ARGB()`](../../src/foundation/core/Inno.Core.Mathematics/Color.cs#L104) | Packs the color channels into an unsigned ARGB value. |

### `Inno.Core.Mathematics.MathHelper`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.MathHelper`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L11) | Contains commonly used precalculated values and mathematical operations. |
| [`const float Inno.Core.Mathematics.MathHelper.C_TOLERANCE`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L16) | The c tolerance value used as part of this type's public representation. |
| [`static bool Inno.Core.Mathematics.MathHelper.AlmostEquals(float a, float b, float relTolerance = 1E-06)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L39) | Determines whether two floating-point numbers are approximately equal, using a relative tolerance based on the magnitude of the numbers. This is useful for comparing floats where rounding errors may occur. |
| [`static bool Inno.Core.Mathematics.MathHelper.IsFinite(float value)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L363) | Returns true if the value is finite (not NaN or infinity). |
| [`static bool Inno.Core.Mathematics.MathHelper.IsPowerOfTwo(int value)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L395) | Determines if value is powered by two. |
| [`static float Inno.Core.Mathematics.MathHelper.Barycentric(float value1, float value2, float value3, float amount1, float amount2)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L69) | Returns the Cartesian coordinate for one axis of a point that is defined by a given triangle and two normalized barycentric (areal) coordinates. |
| [`static float Inno.Core.Mathematics.MathHelper.CatmullRom(float value1, float value2, float value3, float value4, float amount)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L101) | Performs a Catmull-Rom interpolation using the specified positions. |
| [`static float Inno.Core.Mathematics.MathHelper.Clamp(float value, float min, float max)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L335) | Clamps a value to the inclusive range [min, max]. |
| [`static float Inno.Core.Mathematics.MathHelper.Distance(float value1, float value2)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L126) | Calculates the absolute value of the difference of two values. |
| [`static float Inno.Core.Mathematics.MathHelper.Hermite(float value1, float tangent1, float value2, float tangent2, float amount)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L153) | Performs a Hermite spline interpolation. |
| [`static float Inno.Core.Mathematics.MathHelper.Lerp(float value1, float value2, float amount)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L191) | Linearly interpolates between two values. |
| [`static float Inno.Core.Mathematics.MathHelper.LerpPrecise(float value1, float value2, float amount)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L252) | Linearly interpolates between two values. This method is a less efficient, more precise version of . See remarks for more info. |
| [`static float Inno.Core.Mathematics.MathHelper.LerpUnclamped(float value1, float value2, float amount)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L215) | Linearly interpolates between two values without clamping the amount. |
| [`static float Inno.Core.Mathematics.MathHelper.Saturate(float value)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L351) | Clamps a value to the inclusive range [0, 1]. |
| [`static float Inno.Core.Mathematics.MathHelper.SmoothStep(float value1, float value2, float amount)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L276) | Interpolates between two values using a cubic equation. |
| [`static float Inno.Core.Mathematics.MathHelper.ToDegrees(float radians)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L300) | Converts radians to degrees. |
| [`static float Inno.Core.Mathematics.MathHelper.ToRadians(float degrees)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L317) | Converts degrees to radians. |
| [`static float Inno.Core.Mathematics.MathHelper.WrapAngle(float angle)`](../../src/foundation/core/Inno.Core.Mathematics/MathHelper.cs#L375) | Reduces a given angle to a value between π and -π. |

### `Inno.Core.Mathematics.Matrix`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Matrix`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L26) | Represents a mutable four-by-four transformation matrix using column-vector multiplication. |
| [`Inno.Core.Mathematics.Matrix.Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L154) | Initializes a 4x4 matrix with explicit row-major elements. |
| [`bool Inno.Core.Mathematics.Matrix.Equals(Inno.Core.Mathematics.Matrix other)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L1030) | Determines whether this instance is equal to another matrix. |
| [`float Inno.Core.Mathematics.Matrix.m11`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L34) | Row 1, Column 1. |
| [`float Inno.Core.Mathematics.Matrix.m12`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L38) | Row 1, Column 2. |
| [`float Inno.Core.Mathematics.Matrix.m13`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L42) | Row 1, Column 3. |
| [`float Inno.Core.Mathematics.Matrix.m14`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L46) | Row 1, Column 4. |
| [`float Inno.Core.Mathematics.Matrix.m21`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L51) | Row 2, Column 1. |
| [`float Inno.Core.Mathematics.Matrix.m22`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L55) | Row 2, Column 2. |
| [`float Inno.Core.Mathematics.Matrix.m23`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L59) | Row 2, Column 3. |
| [`float Inno.Core.Mathematics.Matrix.m24`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L63) | Row 2, Column 4. |
| [`float Inno.Core.Mathematics.Matrix.m31`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L68) | Row 3, Column 1. |
| [`float Inno.Core.Mathematics.Matrix.m32`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L72) | Row 3, Column 2. |
| [`float Inno.Core.Mathematics.Matrix.m33`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L76) | Row 3, Column 3. |
| [`float Inno.Core.Mathematics.Matrix.m34`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L80) | Row 3, Column 4. |
| [`float Inno.Core.Mathematics.Matrix.m41`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L85) | Row 4, Column 1 (translation X component for affine transforms). |
| [`float Inno.Core.Mathematics.Matrix.m42`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L89) | Row 4, Column 2 (translation Y component for affine transforms). |
| [`float Inno.Core.Mathematics.Matrix.m43`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L93) | Row 4, Column 3 (translation Z component for affine transforms). |
| [`float Inno.Core.Mathematics.Matrix.m44`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L97) | Row 4, Column 4. |
| [`float[] Inno.Core.Mathematics.Matrix.ToColumnMajorArray()`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L896) | Returns a new array containing matrix elements in column-major order. |
| [`override bool Inno.Core.Mathematics.Matrix.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L1041) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Matrix.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L1049) | Computes a hash code from the fields that participate in logical equality. |
| [`override string Inno.Core.Mathematics.Matrix.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L1064) | Returns a multi-line string representation of the matrix in row-major layout. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateFromQuaternion(Inno.Core.Mathematics.Quaternion q)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L347) | Creates a rotation matrix from a quaternion. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateLookAt(Inno.Core.Mathematics.Vector3 eye, Inno.Core.Mathematics.Vector3 target, Inno.Core.Mathematics.Vector3 up)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L531) | Creates a left-handed view matrix that looks from to . |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateLookAtRH(Inno.Core.Mathematics.Vector3 eye, Inno.Core.Mathematics.Vector3 target, Inno.Core.Mathematics.Vector3 up)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L563) | Creates a right-handed view matrix that looks from to . |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateOrthographic(float width, float height, float near, float far)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L450) | Creates an orthographic projection matrix (depth range 0..1). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateOrthographicOffCenter(float left, float right, float bottom, float top, float near, float far)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L489) | Creates an off-center orthographic projection matrix (depth range 0..1). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreatePerspectiveFieldOfView(float fov, float aspect, float near, float far)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L382) | Creates a left-handed, perspective projection matrix ( depth range 0..1). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreatePerspectiveFieldOfViewRH(float fov, float aspect, float near, float far)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L416) | Creates a right-handed, perspective projection matrix (depth range 0..1). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateRotationX(float radians)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L287) | Creates a rotation matrix around the X axis (radians). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateRotationY(float radians)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L307) | Creates a rotation matrix around the Y axis (radians). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateRotationZ(float radians)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L327) | Creates a rotation matrix around the Z axis (radians). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateScale(Inno.Core.Mathematics.Vector3 v)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L276) | Creates a non-uniform scale matrix. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateScale(float scale)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L238) | Creates a uniform scale matrix. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateScale(float x, float y, float z)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L255) | Creates a non-uniform scale matrix. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateTranslation(Inno.Core.Mathematics.Vector3 v)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L227) | Creates a translation matrix. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.CreateTranslation(float x, float y, float z)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L206) | Creates a translation matrix. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.Extract2DTransform(Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L757) | Extracts the 2D-affine portion of a matrix (XY basis + XY translation). |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.Invert(Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L795) | Returns the inverse of a matrix. If the matrix is non-invertible, returns . |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.Multiply(Inno.Core.Mathematics.Matrix a, Inno.Core.Mathematics.Matrix b)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L597) | Multiplies two matrices using standard matrix multiplication. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.Transpose(Inno.Core.Mathematics.Matrix a)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L776) | Returns the transpose of a matrix. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.identity`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L181) | Gets the identity matrix. |
| [`static Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Matrix.operator *(Inno.Core.Mathematics.Matrix a, Inno.Core.Mathematics.Matrix b)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L919) | Multiplies two matrices. |
| [`static Inno.Core.Mathematics.Matrix.implicit operator Inno.Core.Mathematics.Matrix(System.Numerics.Matrix4x4 m)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L1006) | Converts from preserving element order. System.Numerics assumes row-major with row vectors, so transpose as needed for column-vector usage. |
| [`static Inno.Core.Mathematics.Matrix.implicit operator System.Numerics.Matrix4x4(Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L986) | Converts to preserving element order. System.Numerics assumes row-major with row vectors, so transpose as needed for column-vector usage. |
| [`static bool Inno.Core.Mathematics.Matrix.Decompose(Inno.Core.Mathematics.Matrix m, out Inno.Core.Mathematics.Vector3 scale, out Inno.Core.Mathematics.Quaternion rotation, out Inno.Core.Mathematics.Vector3 translation)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L709) | Decomposes a matrix into scale, rotation and translation. |
| [`static bool Inno.Core.Mathematics.Matrix.operator !=(Inno.Core.Mathematics.Matrix a, Inno.Core.Mathematics.Matrix b)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L971) | Tests matrices for inequality. |
| [`static bool Inno.Core.Mathematics.Matrix.operator ==(Inno.Core.Mathematics.Matrix matrix1, Inno.Core.Mathematics.Matrix matrix2)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L936) | Tests matrices for approximate equality (per-element). |
| [`static float Inno.Core.Mathematics.Matrix.Determinant(Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L668) | Returns the determinant of the matrix. |
| [`void Inno.Core.Mathematics.Matrix.CopyToColumnMajor(float[] destination, int startIndex = 0)`](../../src/foundation/core/Inno.Core.Mathematics/Matrix.cs#L858) | Copies matrix elements into an array in column-major order. Useful for APIs like bgfx that expect column-major float[16]. |

### `Inno.Core.Mathematics.Quaternion`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Matrix Inno.Core.Mathematics.Quaternion.ToMatrix()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L309) | Converts this quaternion to a rotation matrix. |
| [`Inno.Core.Mathematics.Quaternion`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L10) | Represents a rotation as a normalized four-component quaternion. |
| [`Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.normalized`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L83) | Gets a unit-length copy, or the zero value when normalization is undefined. |
| [`Inno.Core.Mathematics.Quaternion.Quaternion(float x, float y, float z, float w)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L45) | Creates a validated quaternion instance. |
| [`Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Quaternion.ToEulerAnglesXYZ()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L341) | Converts this value to its euler angles xyz representation. |
| [`Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Quaternion.ToEulerAnglesXYZDegrees()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L364) | Converts this value to its euler angles xyzdegrees representation. |
| [`Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Quaternion.ToEulerAnglesZYX()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L372) | Converts this value to its euler angles zyx representation. |
| [`Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Quaternion.ToEulerAnglesZYXDegrees()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L396) | Converts this value to its euler angles zyxdegrees representation. |
| [`bool Inno.Core.Mathematics.Quaternion.Equals(Inno.Core.Mathematics.Quaternion other)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L633) | Determines whether this value and the supplied value represent the same logical state. |
| [`float Inno.Core.Mathematics.Quaternion.Length()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L68) | Calculates the Euclidean magnitude of this value. |
| [`float Inno.Core.Mathematics.Quaternion.LengthSquared()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L77) | Calculates the squared Euclidean magnitude without a square-root operation. |
| [`float Inno.Core.Mathematics.Quaternion.w`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L28) | The homogeneous or fourth component. |
| [`float Inno.Core.Mathematics.Quaternion.x`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L16) | The horizontal or first component. |
| [`float Inno.Core.Mathematics.Quaternion.y`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L20) | The vertical or second component. |
| [`float Inno.Core.Mathematics.Quaternion.z`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L24) | The depth or third component. |
| [`override bool Inno.Core.Mathematics.Quaternion.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L622) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Quaternion.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L641) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Quaternion.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L649) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.Conjugate(Inno.Core.Mathematics.Quaternion q)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L112) | Returns the quaternion conjugate by negating its vector components. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.CreateFromAxisAngle(Inno.Core.Mathematics.Vector3 axis, float angle)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L202) | Creates and validates a caller-owned from axis angle value. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.CreateFromYawPitchRoll(float yaw, float pitch, float roll)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L327) | Creates and validates a caller-owned from yaw pitch roll value. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.FromEulerAnglesXYZ(Inno.Core.Mathematics.Vector3 euler)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L427) | Creates the target representation from the supplied euler angles xyz value. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.FromEulerAnglesXYZDegrees(Inno.Core.Mathematics.Vector3 eulerDegrees)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L454) | Creates the target representation from the supplied euler angles xyzdegrees value. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.FromEulerAnglesZYX(Inno.Core.Mathematics.Vector3 euler)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L483) | Creates the target representation from the supplied euler angles zyx value. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.FromEulerAnglesZYXDegrees(Inno.Core.Mathematics.Vector3 eulerDegrees)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L510) | Creates the target representation from the supplied euler angles zyxdegrees value. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.FromRotationMatrix(Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L227) | Creates a quaternion from a rotation matrix. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.Inverse(Inno.Core.Mathematics.Quaternion q)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L124) | Calculates the inverse rotation represented by the supplied quaternion. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.LookRotation(Inno.Core.Mathematics.Vector3 forward, Inno.Core.Mathematics.Vector3 up)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L285) | Creates a rotation quaternion that looks in direction with the given . |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.Normalize(Inno.Core.Mathematics.Quaternion q)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L94) | Returns a unit-length value while handling degenerate input according to the method contract. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.Slerp(Inno.Core.Mathematics.Quaternion a, Inno.Core.Mathematics.Quaternion b, float t)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L149) | Interpolates along the shortest spherical path between two rotations. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.identity`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L60) | Gets the stable identity used to reference this value across subsystem boundaries. |
| [`static Inno.Core.Mathematics.Quaternion Inno.Core.Mathematics.Quaternion.operator *(Inno.Core.Mathematics.Quaternion a, Inno.Core.Mathematics.Quaternion b)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L542) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Quaternion.implicit operator Inno.Core.Mathematics.Quaternion(System.Numerics.Quaternion q)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L611) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Quaternion.implicit operator System.Numerics.Quaternion(Inno.Core.Mathematics.Quaternion q)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L601) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Quaternion.operator !=(Inno.Core.Mathematics.Quaternion a, Inno.Core.Mathematics.Quaternion b)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L587) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Quaternion.operator ==(Inno.Core.Mathematics.Quaternion a, Inno.Core.Mathematics.Quaternion b)`](../../src/foundation/core/Inno.Core.Mathematics/Quaternion.cs#L566) | Determines whether the supplied values are equal under the type's equality tolerance. |

### `Inno.Core.Mathematics.Rect`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Rect`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L10) | Represents an axis-aligned rectangle with float coordinates. Coordinates assume Y-axis points upwards (top smaller than bottom). |
| [`Inno.Core.Mathematics.Rect.Rect(float x, float y, float width, float height)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L66) | Creates a validated rect instance. |
| [`Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Rect.center`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L49) | Gets the midpoint derived from the rectangle bounds. |
| [`Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Rect.max`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L41) | Gets the maximum corner of this axis-aligned rectangle. |
| [`Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Rect.min`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L37) | Gets the minimum corner of this axis-aligned rectangle. |
| [`Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Rect.size`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L45) | Gets the width and height derived from the rectangle bounds. |
| [`bool Inno.Core.Mathematics.Rect.Contains(Inno.Core.Mathematics.Rect other)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L105) | Checks if this rectangle fully contains another rectangle. |
| [`bool Inno.Core.Mathematics.Rect.Contains(Inno.Core.Mathematics.Vector2 p)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L144) | Determines whether current state contains the requested value value. |
| [`bool Inno.Core.Mathematics.Rect.Contains(float px, float py)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L126) | Checks if this rectangle contains a point. |
| [`bool Inno.Core.Mathematics.Rect.Equals(Inno.Core.Mathematics.Rect other)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L340) | Determines whether this value and the supplied value represent the same logical state. |
| [`bool Inno.Core.Mathematics.Rect.Overlaps(Inno.Core.Mathematics.Rect other)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L87) | Checks if this rectangle overlaps another rectangle. |
| [`float Inno.Core.Mathematics.Rect.bottom`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L32) | Gets the scalar measurement or identity associated with the current state. |
| [`float Inno.Core.Mathematics.Rect.height`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L15) | The horizontal or first component. |
| [`float Inno.Core.Mathematics.Rect.left`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L20) | Gets the scalar measurement or identity associated with the current state. |
| [`float Inno.Core.Mathematics.Rect.right`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L24) | Gets the scalar measurement or identity associated with the current state. |
| [`float Inno.Core.Mathematics.Rect.top`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L28) | Gets the scalar measurement or identity associated with the current state. |
| [`float Inno.Core.Mathematics.Rect.width`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L15) | The horizontal or first component. |
| [`float Inno.Core.Mathematics.Rect.x`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L15) | The horizontal or first component. |
| [`float Inno.Core.Mathematics.Rect.y`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L15) | The horizontal or first component. |
| [`override bool Inno.Core.Mathematics.Rect.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L330) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Rect.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L351) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Rect.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L358) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Rect Inno.Core.Mathematics.Rect.FromMinMax(Inno.Core.Mathematics.Vector2 min, Inno.Core.Mathematics.Vector2 max)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L159) | Creates a rectangle from inclusive minimum and maximum corner values. |
| [`static Inno.Core.Mathematics.Rect Inno.Core.Mathematics.Rect.Union(Inno.Core.Mathematics.Rect a, Inno.Core.Mathematics.Rect b)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L179) | Returns the smallest axis-aligned rectangle containing both supplied rectangles. |
| [`static Inno.Core.Mathematics.Rect Inno.Core.Mathematics.Rect.operator +(Inno.Core.Mathematics.Rect a, Inno.Core.Mathematics.Rect b)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L274) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.Rect Inno.Core.Mathematics.Rect.operator -(Inno.Core.Mathematics.Rect a, Inno.Core.Mathematics.Rect b)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L292) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Rect.implicit operator Inno.Core.Mathematics.Rect(System.Numerics.Vector4 v)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L318) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Rect.implicit operator System.Numerics.Vector4(Inno.Core.Mathematics.Rect r)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L307) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Rect.TryIntersect(Inno.Core.Mathematics.Rect a, Inno.Core.Mathematics.Rect b, out Inno.Core.Mathematics.Rect intersection)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L206) | Attempts to intersect without changing state when the operation cannot complete. |
| [`static bool Inno.Core.Mathematics.Rect.operator !=(Inno.Core.Mathematics.Rect a, Inno.Core.Mathematics.Rect b)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L256) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Rect.operator ==(Inno.Core.Mathematics.Rect a, Inno.Core.Mathematics.Rect b)`](../../src/foundation/core/Inno.Core.Mathematics/Rect.cs#L239) | Determines whether the supplied values are equal under the type's equality tolerance. |

### `Inno.Core.Mathematics.RectInt`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.RectInt`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L11) | Represents an axis-aligned rectangle with integer coordinates. Coordinates assume Y-axis points upwards (top smaller than bottom). |
| [`Inno.Core.Mathematics.RectInt.RectInt(int x, int y, int width, int height)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L67) | Creates a validated rect int instance. |
| [`Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.RectInt.center`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L50) | Gets the midpoint derived from the rectangle bounds. |
| [`Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.RectInt.max`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L42) | Gets the maximum corner of this axis-aligned rectangle. |
| [`Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.RectInt.min`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L38) | Gets the minimum corner of this axis-aligned rectangle. |
| [`Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.RectInt.size`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L46) | Gets the width and height derived from the rectangle bounds. |
| [`bool Inno.Core.Mathematics.RectInt.Contains(Inno.Core.Mathematics.RectInt other)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L106) | Checks if this rectangle fully contains another rectangle. |
| [`bool Inno.Core.Mathematics.RectInt.Contains(Inno.Core.Mathematics.Vector2Int p)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L145) | Determines whether current state contains the requested value value. |
| [`bool Inno.Core.Mathematics.RectInt.Contains(int px, int py)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L127) | Checks if this rectangle contains a point. |
| [`bool Inno.Core.Mathematics.RectInt.Equals(Inno.Core.Mathematics.RectInt other)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L366) | Determines whether this value and the supplied value represent the same logical state. |
| [`bool Inno.Core.Mathematics.RectInt.Overlaps(Inno.Core.Mathematics.RectInt other)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L88) | Checks if this rectangle overlaps another rectangle. |
| [`int Inno.Core.Mathematics.RectInt.bottom`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L33) | Gets the scalar measurement or identity associated with the current state. |
| [`int Inno.Core.Mathematics.RectInt.height`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L16) | The horizontal or first component. |
| [`int Inno.Core.Mathematics.RectInt.left`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L21) | Gets the scalar measurement or identity associated with the current state. |
| [`int Inno.Core.Mathematics.RectInt.right`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L25) | Gets the scalar measurement or identity associated with the current state. |
| [`int Inno.Core.Mathematics.RectInt.top`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L29) | Gets the scalar measurement or identity associated with the current state. |
| [`int Inno.Core.Mathematics.RectInt.width`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L16) | The horizontal or first component. |
| [`int Inno.Core.Mathematics.RectInt.x`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L16) | The horizontal or first component. |
| [`int Inno.Core.Mathematics.RectInt.y`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L16) | The horizontal or first component. |
| [`override bool Inno.Core.Mathematics.RectInt.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L356) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.RectInt.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L373) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.RectInt.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L380) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.RectInt Inno.Core.Mathematics.RectInt.FromMinMax(Inno.Core.Mathematics.Vector2Int min, Inno.Core.Mathematics.Vector2Int max)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L160) | Creates a rectangle from inclusive minimum and maximum corner values. |
| [`static Inno.Core.Mathematics.RectInt Inno.Core.Mathematics.RectInt.Union(Inno.Core.Mathematics.RectInt a, Inno.Core.Mathematics.RectInt b)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L180) | Returns the smallest axis-aligned rectangle containing both supplied rectangles. |
| [`static Inno.Core.Mathematics.RectInt Inno.Core.Mathematics.RectInt.operator +(Inno.Core.Mathematics.RectInt a, Inno.Core.Mathematics.RectInt b)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L275) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.RectInt Inno.Core.Mathematics.RectInt.operator -(Inno.Core.Mathematics.RectInt a, Inno.Core.Mathematics.RectInt b)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L293) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.RectInt.implicit operator Inno.Core.Mathematics.RectInt(Inno.Core.Mathematics.Vector4Int v)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L344) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.RectInt.implicit operator Inno.Core.Mathematics.RectInt(System.Drawing.Rectangle r)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L320) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.RectInt.implicit operator Inno.Core.Mathematics.Vector4Int(Inno.Core.Mathematics.RectInt r)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L332) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.RectInt.implicit operator System.Drawing.Rectangle(Inno.Core.Mathematics.RectInt r)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L308) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.RectInt.TryIntersect(Inno.Core.Mathematics.RectInt a, Inno.Core.Mathematics.RectInt b, out Inno.Core.Mathematics.RectInt intersection)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L207) | Attempts to intersect without changing state when the operation cannot complete. |
| [`static bool Inno.Core.Mathematics.RectInt.operator !=(Inno.Core.Mathematics.RectInt a, Inno.Core.Mathematics.RectInt b)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L257) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.RectInt.operator ==(Inno.Core.Mathematics.RectInt a, Inno.Core.Mathematics.RectInt b)`](../../src/foundation/core/Inno.Core.Mathematics/RectInt.cs#L240) | Determines whether the supplied values are equal under the type's equality tolerance. |

### `Inno.Core.Mathematics.Vector2`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector2`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L10) | Represents a mutable vector2 value with component-wise arithmetic semantics. |
| [`Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.normalized`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L77) | Gets a unit-length copy, or the zero value when normalization is undefined. |
| [`Inno.Core.Mathematics.Vector2.Vector2(float x, float y)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L31) | Creates a vector from explicit component values. |
| [`bool Inno.Core.Mathematics.Vector2.Equals(Inno.Core.Mathematics.Vector2 other)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L501) | Determines whether this value and the supplied value represent the same logical state. |
| [`float Inno.Core.Mathematics.Vector2.Length()`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L62) | Calculates the Euclidean magnitude of this value. |
| [`float Inno.Core.Mathematics.Vector2.LengthSquared()`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L71) | Calculates the squared Euclidean magnitude without a square-root operation. |
| [`float Inno.Core.Mathematics.Vector2.x`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L16) | The horizontal or first component. |
| [`float Inno.Core.Mathematics.Vector2.y`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L20) | The vertical or second component. |
| [`override bool Inno.Core.Mathematics.Vector2.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L491) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Vector2.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L508) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Vector2.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L515) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.Lerp(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b, float t)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L216) | Interpolates linearly between two values without clamping the interpolation factor. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.Max(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L253) | Selects the maximum value independently for each component. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.Min(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L235) | Selects the minimum value independently for each component. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.NormalizeSafe(Inno.Core.Mathematics.Vector2 value, float epsilon = 1E-06)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L98) | Returns a unit-length value while handling degenerate input according to the method contract. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.Project(Inno.Core.Mathematics.Vector2 vector, Inno.Core.Mathematics.Vector2 onto)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L187) | Projects the supplied value onto the requested dimensional space. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.Reflect(Inno.Core.Mathematics.Vector2 v, Inno.Core.Mathematics.Vector2 n)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L271) | Reflects an incident value across the supplied normal. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.Transform(Inno.Core.Mathematics.Vector2 v, Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L289) | Transforms the supplied value by the requested transformation. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.Transform(Inno.Core.Mathematics.Vector2 value, Inno.Core.Mathematics.Quaternion rotation)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L311) | Transforms the supplied value by the requested transformation. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.operator *(Inno.Core.Mathematics.Vector2 v, float scalar)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L386) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.operator *(float scalar, Inno.Core.Mathematics.Vector2 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L403) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.operator +(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L341) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.operator -(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L358) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.operator -(Inno.Core.Mathematics.Vector2 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L372) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.operator /(Inno.Core.Mathematics.Vector2 v, float scalar)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L420) | Divides the supplied value by the scalar divisor component by component. |
| [`static Inno.Core.Mathematics.Vector2.implicit operator Inno.Core.Mathematics.Vector2(System.Numerics.Vector2 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L480) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Vector2.implicit operator System.Numerics.Vector2(Inno.Core.Mathematics.Vector2 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L470) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Vector2.operator !=(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L455) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Vector2.operator ==(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L438) | Determines whether the supplied values are equal under the type's equality tolerance. |
| [`static float Inno.Core.Mathematics.Vector2.Angle(Inno.Core.Mathematics.Vector2 from, Inno.Core.Mathematics.Vector2 to)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L137) | Calculates the unsigned angle in radians between two values. |
| [`static float Inno.Core.Mathematics.Vector2.Dot(Inno.Core.Mathematics.Vector2 a, Inno.Core.Mathematics.Vector2 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L119) | Calculates the scalar dot product of two values. |
| [`static float Inno.Core.Mathematics.Vector2.SignedAngle(Inno.Core.Mathematics.Vector2 from, Inno.Core.Mathematics.Vector2 to)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L165) | Calculates the signed angle in radians from one value to another. |
| [`static readonly Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.ONE`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L46) | A value whose components are all one. |
| [`static readonly Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.UNIT_X`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L50) | A unit value aligned with the x axis. |
| [`static readonly Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.UNIT_Y`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L54) | A unit value aligned with the y axis. |
| [`static readonly Inno.Core.Mathematics.Vector2 Inno.Core.Mathematics.Vector2.ZERO`](../../src/foundation/core/Inno.Core.Mathematics/Vector2.cs#L42) | A value whose components are all zero. |

### `Inno.Core.Mathematics.Vector2Int`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector2Int`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L10) | Represents a mutable vector2int value with component-wise arithmetic semantics. |
| [`Inno.Core.Mathematics.Vector2Int.Vector2Int(int x, int y)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L31) | Creates a vector from explicit component values. |
| [`bool Inno.Core.Mathematics.Vector2Int.Equals(Inno.Core.Mathematics.Vector2Int other)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L242) | Determines whether this value and the supplied value represent the same logical state. |
| [`int Inno.Core.Mathematics.Vector2Int.x`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L16) | The horizontal or first component. |
| [`int Inno.Core.Mathematics.Vector2Int.y`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L20) | The vertical or second component. |
| [`override bool Inno.Core.Mathematics.Vector2Int.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L231) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Vector2Int.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L250) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Vector2Int.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L258) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.operator *(Inno.Core.Mathematics.Vector2Int v, int scalar)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L118) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.operator *(int scalar, Inno.Core.Mathematics.Vector2Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L136) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.operator +(Inno.Core.Mathematics.Vector2Int a, Inno.Core.Mathematics.Vector2Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L70) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.operator -(Inno.Core.Mathematics.Vector2Int a, Inno.Core.Mathematics.Vector2Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L88) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.operator -(Inno.Core.Mathematics.Vector2Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L103) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.operator /(Inno.Core.Mathematics.Vector2Int v, int scalar)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L154) | Divides the supplied value by the scalar divisor component by component. |
| [`static Inno.Core.Mathematics.Vector2Int.explicit operator Inno.Core.Mathematics.Vector2(Inno.Core.Mathematics.Vector2Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L206) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Vector2Int.explicit operator Inno.Core.Mathematics.Vector2Int(Inno.Core.Mathematics.Vector2 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L218) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Vector2Int.operator !=(Inno.Core.Mathematics.Vector2Int a, Inno.Core.Mathematics.Vector2Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L190) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Vector2Int.operator ==(Inno.Core.Mathematics.Vector2Int a, Inno.Core.Mathematics.Vector2Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L172) | Determines whether the supplied values are equal under the type's equality tolerance. |
| [`static readonly Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.ONE`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L47) | A value whose components are all one. |
| [`static readonly Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.UNIT_X`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L51) | A unit value aligned with the x axis. |
| [`static readonly Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.UNIT_Y`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L55) | A unit value aligned with the y axis. |
| [`static readonly Inno.Core.Mathematics.Vector2Int Inno.Core.Mathematics.Vector2Int.ZERO`](../../src/foundation/core/Inno.Core.Mathematics/Vector2Int.cs#L43) | A value whose components are all zero. |

### `Inno.Core.Mathematics.Vector3`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector3`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L13) | Represents a mutable vector3 value with component-wise arithmetic semantics. |
| [`Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.normalized`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L107) | Gets a unit-length copy, or the zero value when normalization is undefined. |
| [`Inno.Core.Mathematics.Vector3.Vector3(float x, float y, float z)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L41) | Creates a vector from explicit component values. |
| [`bool Inno.Core.Mathematics.Vector3.Equals(Inno.Core.Mathematics.Vector3 other)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L601) | Determines whether this value and the supplied value represent the same logical state. |
| [`float Inno.Core.Mathematics.Vector3.Length()`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L92) | Calculates the Euclidean magnitude of this value. |
| [`float Inno.Core.Mathematics.Vector3.LengthSquared()`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L101) | Calculates the squared Euclidean magnitude without a square-root operation. |
| [`float Inno.Core.Mathematics.Vector3.x`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L19) | The horizontal or first component. |
| [`float Inno.Core.Mathematics.Vector3.y`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L23) | The vertical or second component. |
| [`float Inno.Core.Mathematics.Vector3.z`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L27) | The depth or third component. |
| [`override bool Inno.Core.Mathematics.Vector3.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L591) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Vector3.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L608) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Vector3.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L615) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.Cross(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L250) | Calculates the vector perpendicular to both supplied vectors. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.Lerp(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b, float t)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L295) | Interpolates linearly between two values without clamping the interpolation factor. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.NormalizeSafe(Inno.Core.Mathematics.Vector3 value, float epsilon = 1E-06)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L128) | Returns a unit-length value while handling degenerate input according to the method contract. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.Project(Inno.Core.Mathematics.Vector3 vector, Inno.Core.Mathematics.Vector3 onto)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L223) | Projects the supplied value onto the requested dimensional space. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.Reflect(Inno.Core.Mathematics.Vector3 dir, Inno.Core.Mathematics.Vector3 normal)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L316) | Reflects an incident value across the supplied normal. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.Transform(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Matrix matrix)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L333) | The column vector transform. It performs as below m * v (not v * m). |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.Transform(Inno.Core.Mathematics.Vector3 value, Inno.Core.Mathematics.Quaternion rotation)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L405) | Transforms the supplied value by the requested transformation. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.TransformNormal(Inno.Core.Mathematics.Vector3 normal, Inno.Core.Mathematics.Matrix matrix)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L369) | Transforms a normal by a matrix (ignores translation). |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.operator *(Inno.Core.Mathematics.Vector3 v, float s)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L482) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.operator *(float s, Inno.Core.Mathematics.Vector3 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L499) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.operator +(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L437) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.operator -(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L454) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.operator -(Inno.Core.Mathematics.Vector3 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L468) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.operator /(Inno.Core.Mathematics.Vector3 v, float s)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L516) | Divides the supplied value by the scalar divisor component by component. |
| [`static Inno.Core.Mathematics.Vector3.implicit operator Inno.Core.Mathematics.Vector3(System.Numerics.Vector3 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L580) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Vector3.implicit operator System.Numerics.Vector3(Inno.Core.Mathematics.Vector3 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L570) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Vector3.operator !=(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L555) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Vector3.operator ==(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L534) | Determines whether the supplied values are equal under the type's equality tolerance. |
| [`static float Inno.Core.Mathematics.Vector3.Angle(Inno.Core.Mathematics.Vector3 from, Inno.Core.Mathematics.Vector3 to)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L168) | Calculates the unsigned angle in radians between two values. |
| [`static float Inno.Core.Mathematics.Vector3.Distance(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L273) | Calculates the Euclidean distance between two points. |
| [`static float Inno.Core.Mathematics.Vector3.Dot(Inno.Core.Mathematics.Vector3 a, Inno.Core.Mathematics.Vector3 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L150) | Calculates the scalar dot product of two values. |
| [`static float Inno.Core.Mathematics.Vector3.SignedAngle(Inno.Core.Mathematics.Vector3 from, Inno.Core.Mathematics.Vector3 to, Inno.Core.Mathematics.Vector3 axis)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L199) | Calculates the signed angle in radians from one value to another. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.BACK`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L83) | The back value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.DOWN`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L67) | The down value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.FORWARD`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L79) | The forward value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.LEFT`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L71) | The left value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.ONE`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L59) | A value whose components are all one. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.RIGHT`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L75) | The right value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.UP`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L63) | The up value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector3.ZERO`](../../src/foundation/core/Inno.Core.Mathematics/Vector3.cs#L55) | A value whose components are all zero. |

### `Inno.Core.Mathematics.Vector3Int`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector3Int`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L10) | Represents a mutable vector3int value with component-wise arithmetic semantics. |
| [`Inno.Core.Mathematics.Vector3Int.Vector3Int(int x, int y, int z)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L38) | Creates a vector from explicit component values. |
| [`bool Inno.Core.Mathematics.Vector3Int.Equals(Inno.Core.Mathematics.Vector3Int other)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L263) | Determines whether this value and the supplied value represent the same logical state. |
| [`int Inno.Core.Mathematics.Vector3Int.x`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L16) | The horizontal or first component. |
| [`int Inno.Core.Mathematics.Vector3Int.y`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L20) | The vertical or second component. |
| [`int Inno.Core.Mathematics.Vector3Int.z`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L24) | The depth or third component. |
| [`override bool Inno.Core.Mathematics.Vector3Int.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L252) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Vector3Int.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L271) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Vector3Int.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L279) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.operator *(Inno.Core.Mathematics.Vector3Int v, int scalar)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L141) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.operator *(int scalar, Inno.Core.Mathematics.Vector3Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L159) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.operator +(Inno.Core.Mathematics.Vector3Int a, Inno.Core.Mathematics.Vector3Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L93) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.operator -(Inno.Core.Mathematics.Vector3Int a, Inno.Core.Mathematics.Vector3Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L111) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.operator -(Inno.Core.Mathematics.Vector3Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L126) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.operator /(Inno.Core.Mathematics.Vector3Int v, int scalar)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L177) | Divides the supplied value by the scalar divisor component by component. |
| [`static Inno.Core.Mathematics.Vector3Int.explicit operator Inno.Core.Mathematics.Vector3(Inno.Core.Mathematics.Vector3Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L228) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Vector3Int.explicit operator Inno.Core.Mathematics.Vector3Int(Inno.Core.Mathematics.Vector3 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L240) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Vector3Int.operator !=(Inno.Core.Mathematics.Vector3Int a, Inno.Core.Mathematics.Vector3Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L213) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Vector3Int.operator ==(Inno.Core.Mathematics.Vector3Int a, Inno.Core.Mathematics.Vector3Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L195) | Determines whether the supplied values are equal under the type's equality tolerance. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.BACK`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L79) | The back value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.DOWN`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L63) | The down value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.FORWARD`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L75) | The forward value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.LEFT`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L67) | The left value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.ONE`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L55) | A value whose components are all one. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.RIGHT`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L71) | The right value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.UP`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L59) | The up value used as part of this type's public representation. |
| [`static readonly Inno.Core.Mathematics.Vector3Int Inno.Core.Mathematics.Vector3Int.ZERO`](../../src/foundation/core/Inno.Core.Mathematics/Vector3Int.cs#L51) | A value whose components are all zero. |

### `Inno.Core.Mathematics.Vector4`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector3 Inno.Core.Mathematics.Vector4.ProjectToVector3()`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L228) | Projects the supplied value onto the requested dimensional space. |
| [`Inno.Core.Mathematics.Vector4`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L13) | Represents a mutable vector4 value with component-wise arithmetic semantics. |
| [`Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.normalized`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L108) | Gets a unit-length copy, or the zero value when normalization is undefined. |
| [`Inno.Core.Mathematics.Vector4.Vector4(float x, float y, float z, float w)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L48) | Creates a vector from explicit component values. |
| [`bool Inno.Core.Mathematics.Vector4.Equals(Inno.Core.Mathematics.Vector4 other)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L449) | Determines whether this value and the supplied value represent the same logical state. |
| [`float Inno.Core.Mathematics.Vector4.Length()`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L93) | Calculates the Euclidean magnitude of this value. |
| [`float Inno.Core.Mathematics.Vector4.LengthSquared()`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L102) | Calculates the squared Euclidean magnitude without a square-root operation. |
| [`float Inno.Core.Mathematics.Vector4.w`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L31) | The homogeneous or fourth component. |
| [`float Inno.Core.Mathematics.Vector4.x`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L19) | The horizontal or first component. |
| [`float Inno.Core.Mathematics.Vector4.y`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L23) | The vertical or second component. |
| [`float Inno.Core.Mathematics.Vector4.z`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L27) | The depth or third component. |
| [`override bool Inno.Core.Mathematics.Vector4.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L439) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Vector4.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L456) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Vector4.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L463) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.Lerp(Inno.Core.Mathematics.Vector4 a, Inno.Core.Mathematics.Vector4 b, float t)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L153) | Interpolates linearly between two values without clamping the interpolation factor. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.Reflect(Inno.Core.Mathematics.Vector4 vec, Inno.Core.Mathematics.Vector4 normal)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L174) | Reflects an incident value across the supplied normal. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.Transform(Inno.Core.Mathematics.Vector4 v, Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L194) | Transforms the supplied value by the requested transformation. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.operator *(Inno.Core.Mathematics.Matrix m, Inno.Core.Mathematics.Vector4 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L337) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.operator *(Inno.Core.Mathematics.Vector4 v, float s)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L300) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.operator *(float s, Inno.Core.Mathematics.Vector4 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L319) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.operator +(Inno.Core.Mathematics.Vector4 a, Inno.Core.Mathematics.Vector4 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L249) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.operator -(Inno.Core.Mathematics.Vector4 a, Inno.Core.Mathematics.Vector4 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L268) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.operator -(Inno.Core.Mathematics.Vector4 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L284) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.operator /(Inno.Core.Mathematics.Vector4 v, float s)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L362) | Divides the supplied value by the scalar divisor component by component. |
| [`static Inno.Core.Mathematics.Vector4.implicit operator Inno.Core.Mathematics.Vector4(System.Numerics.Vector4 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L428) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Vector4.implicit operator System.Numerics.Vector4(Inno.Core.Mathematics.Vector4 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L418) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Vector4.operator !=(Inno.Core.Mathematics.Vector4 a, Inno.Core.Mathematics.Vector4 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L403) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Vector4.operator ==(Inno.Core.Mathematics.Vector4 a, Inno.Core.Mathematics.Vector4 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L381) | Determines whether the supplied values are equal under the type's equality tolerance. |
| [`static float Inno.Core.Mathematics.Vector4.Dot(Inno.Core.Mathematics.Vector4 a, Inno.Core.Mathematics.Vector4 b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L130) | Calculates the scalar dot product of two values. |
| [`static readonly Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.ONE`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L68) | A value whose components are all one. |
| [`static readonly Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.UNIT_W`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L84) | A unit value aligned with the w axis. |
| [`static readonly Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.UNIT_X`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L72) | A unit value aligned with the x axis. |
| [`static readonly Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.UNIT_Y`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L76) | A unit value aligned with the y axis. |
| [`static readonly Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.UNIT_Z`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L80) | A unit value aligned with the z axis. |
| [`static readonly Inno.Core.Mathematics.Vector4 Inno.Core.Mathematics.Vector4.ZERO`](../../src/foundation/core/Inno.Core.Mathematics/Vector4.cs#L64) | A value whose components are all zero. |

### `Inno.Core.Mathematics.Vector4Int`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector4Int`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L10) | Represents a mutable vector4int value with component-wise arithmetic semantics. |
| [`Inno.Core.Mathematics.Vector4Int.Vector4Int(int x, int y, int z, int w)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L45) | Creates a vector from explicit component values. |
| [`bool Inno.Core.Mathematics.Vector4Int.Equals(Inno.Core.Mathematics.Vector4Int other)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L372) | Determines whether this value and the supplied value represent the same logical state. |
| [`int Inno.Core.Mathematics.Vector4Int.w`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L28) | The homogeneous or fourth component. |
| [`int Inno.Core.Mathematics.Vector4Int.x`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L16) | The horizontal or first component. |
| [`int Inno.Core.Mathematics.Vector4Int.y`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L20) | The vertical or second component. |
| [`int Inno.Core.Mathematics.Vector4Int.z`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L24) | The depth or third component. |
| [`override bool Inno.Core.Mathematics.Vector4Int.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L362) | Determines whether this value and the supplied value represent the same logical state. |
| [`override int Inno.Core.Mathematics.Vector4Int.GetHashCode()`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L379) | Computes a hash code consistent with the implemented equality contract. |
| [`override string Inno.Core.Mathematics.Vector4Int.ToString()`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L386) | Formats this value as a human-readable component list. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.Lerp(Inno.Core.Mathematics.Vector4Int a, Inno.Core.Mathematics.Vector4Int b, float t)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L119) | Interpolates linearly between two values without clamping the interpolation factor. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.Reflect(Inno.Core.Mathematics.Vector4Int vec, Inno.Core.Mathematics.Vector4Int normal)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L147) | Reflects an incident value across the supplied normal. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.Transform(Inno.Core.Mathematics.Vector4Int v, Inno.Core.Mathematics.Matrix m)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L169) | Transforms the supplied value by the requested transformation. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.operator *(Inno.Core.Mathematics.Vector4Int v, int s)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L245) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.operator *(int s, Inno.Core.Mathematics.Vector4Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L264) | Multiplies the supplied values according to their algebraic contract. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.operator +(Inno.Core.Mathematics.Vector4Int a, Inno.Core.Mathematics.Vector4Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L194) | Adds the supplied values component by component. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.operator -(Inno.Core.Mathematics.Vector4Int a, Inno.Core.Mathematics.Vector4Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L213) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.operator -(Inno.Core.Mathematics.Vector4Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L229) | Subtracts or negates the supplied value component by component. |
| [`static Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.operator /(Inno.Core.Mathematics.Vector4Int v, int s)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L282) | Divides the supplied value by the scalar divisor component by component. |
| [`static Inno.Core.Mathematics.Vector4Int.explicit operator Inno.Core.Mathematics.Vector4(Inno.Core.Mathematics.Vector4Int v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L336) | Converts the supplied value to . |
| [`static Inno.Core.Mathematics.Vector4Int.explicit operator Inno.Core.Mathematics.Vector4Int(Inno.Core.Mathematics.Vector4 v)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L349) | Converts the supplied value to . |
| [`static bool Inno.Core.Mathematics.Vector4Int.operator !=(Inno.Core.Mathematics.Vector4Int a, Inno.Core.Mathematics.Vector4Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L320) | Determines whether the supplied values differ under the type's equality tolerance. |
| [`static bool Inno.Core.Mathematics.Vector4Int.operator ==(Inno.Core.Mathematics.Vector4Int a, Inno.Core.Mathematics.Vector4Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L301) | Determines whether the supplied values are equal under the type's equality tolerance. |
| [`static int Inno.Core.Mathematics.Vector4Int.Dot(Inno.Core.Mathematics.Vector4Int a, Inno.Core.Mathematics.Vector4Int b)`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L96) | Calculates the scalar dot product of two values. |
| [`static readonly Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.ONE`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L65) | A value whose components are all one. |
| [`static readonly Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.UNIT_W`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L81) | A unit value aligned with the w axis. |
| [`static readonly Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.UNIT_X`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L69) | A unit value aligned with the x axis. |
| [`static readonly Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.UNIT_Y`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L73) | A unit value aligned with the y axis. |
| [`static readonly Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.UNIT_Z`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L77) | A unit value aligned with the z axis. |
| [`static readonly Inno.Core.Mathematics.Vector4Int Inno.Core.Mathematics.Vector4Int.ZERO`](../../src/foundation/core/Inno.Core.Mathematics/Vector4Int.cs#L61) | A value whose components are all zero. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
