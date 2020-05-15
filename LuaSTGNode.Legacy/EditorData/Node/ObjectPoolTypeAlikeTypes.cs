using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class ObjectPoolTypeAlikeTypes : ITypeEnumerable
    {
        private static readonly Type[] types =
            { typeof(Bullet.BulletDefine), typeof(Laser.LaserDefine), typeof(Laser.BentLaserDefine), typeof(Object.ObjectDefine) 
            , typeof(Enemy.EnemyDefine) };

        public IEnumerator<Type> GetEnumerator()
        {
            foreach (Type t in types)
            {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
