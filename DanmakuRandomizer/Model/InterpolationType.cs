using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmakuRandomizer.Model
{
    /// <summary>
    /// Store interpolation types.
    /// <para/>
    /// 7 types are supported:
    /// <para/>
    /// <see cref="Sine"/> | <see cref="Accelerate"/> (or <see cref="Accelerate"/>, 1)
    /// means using Sine Interpolation Variable with ACCEL parameter
    /// <para/>
    /// <see cref="Sine"/> | <see cref="Decelerate"/> (or <see cref="Decelerate"/>, 2)
    /// means using Sine Interpolation Variable with DECEL parameter
    /// <para/>
    /// <see cref="Sine"/> | <see cref="Acc_Dec"/> (or <see cref="Acc_Dec"/>, 3)
    /// means using Sine Interpolation Variable with ACC_DEC parameter
    /// <para/>
    /// <see cref="Linear"/> (or 4)
    /// means using Linear Interpolation Variable with NORMAL parameter
    /// <para/>
    /// <see cref="Linear"/> | <see cref="Accelerate"/> (or 5)
    /// means using Linear Interpolation Variable with ACCEL parameter
    /// <para/>
    /// <see cref="Linear"/> | <see cref="Decelerate"/> (or 6)
    /// means using Linear Interpolation Variable with DECEL parameter
    /// <para/>
    /// <see cref="Linear"/> | <see cref="Acc_Dec"/> (or 7)
    /// means using Linear Interpolation Variable with ACC_DEC parameter
    /// </summary>
    internal enum InterpolationType
    {
        Linear = 4,
        Sine = 0,
        Accelerate = 1,
        Decelerate = 2,
        Acc_Dec = 3
    }
}
