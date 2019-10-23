using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class CodeAlikeTypes : ITypeEnumerable
    {
        private static readonly Type[] types =
            { typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
            , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
            , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit), typeof(Data.Function), typeof(Object.ObjectDefine)
            , typeof(Task.TaskDefine), typeof(Boss.BossInit), typeof(Render.OnRender), typeof(Boss.Dialog)
            , typeof(Enemy.EnemyInit) };

        public IEnumerator<Type> GetEnumerator()
        {
            foreach(Type t in types)
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
