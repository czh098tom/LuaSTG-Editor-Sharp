using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.Windows
{
    public partial class ViewDefinition : ViewDefinitionBase
    {
        public override void InitializeTree()
        {
            base.InitializeTree();
            Tree.Add(GetStageGroupMeta());
            Tree.Add(GetEditorObjMeta(MetaType.Bullet, "Bullet", "bulletdefine.png"));
            Tree.Add(GetEditorObjMeta(MetaType.Laser, "Laser", "laserdefine.png"));
            Tree.Add(GetEditorObjMeta(MetaType.BentLaser, "Bent Laser", "laserbentdefine.png"));
            Tree.Add(GetEditorObjMeta(MetaType.Object, "Object", "objectdefine.png"));
            Tree.Add(GetTaskMeta());
            Tree.Add(GetBossMeta());
            Tree.Add(GetImageMeta());
            Tree.Add(GetImageGroupMeta());
            Tree.Add(GetBGMMeta());
            Tree.Add(GetBossBGDefineMeta());
        }

        private MetaModel GetStageGroupMeta()
        {
            MetaModel stageGroup = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/stagegroup.png",
                Text = "Stage Groups"
            };
            var a = data.Meta.aggregatableMetas[(int)MetaType.StageGroup].GetAllFullWithDifficulty("");
            foreach (MetaModel info in a)
            {
                stageGroup.Children.Add(info);
            }

            return stageGroup;
        }

        private MetaModel GetBulletMeta()
        {
            MetaModel bullet = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/bulletdefine.png",
                Text = "Bullets"
            };

            var bulletEMeta = data.Meta.aggregatableMetas[(int)MetaType.Bullet].GetAllFullWithDifficulty("Easy");
            if (bulletEMeta.Count > 0)
            {
                MetaModel bulletE = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Easy"
                };
                foreach (MetaModel info in bulletEMeta)
                {
                    bulletE.Children.Add(info);
                }
                bullet.Children.Add(bulletE);
            }

            var bulletNMeta = data.Meta.aggregatableMetas[(int)MetaType.Bullet].GetAllFullWithDifficulty("Normal");
            if (bulletNMeta.Count > 0)
            {
                MetaModel bulletN = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Normal"
                };
                foreach (MetaModel info in bulletNMeta)
                {
                    bulletN.Children.Add(info);
                }
                bullet.Children.Add(bulletN);
            }

            var bulletHMeta = data.Meta.aggregatableMetas[(int)MetaType.Bullet].GetAllFullWithDifficulty("Hard");
            if (bulletHMeta.Count > 0)
            {
                MetaModel bulletH = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Hard"
                };
                foreach (MetaModel info in bulletHMeta)
                {
                    bulletH.Children.Add(info);
                }
                bullet.Children.Add(bulletH);
            }

            var bulletLMeta = data.Meta.aggregatableMetas[(int)MetaType.Bullet].GetAllFullWithDifficulty("Lunatic");
            if (bulletLMeta.Count > 0)
            {
                MetaModel bulletL = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Lunatic"
                };
                foreach (MetaModel info in bulletLMeta)
                {
                    bulletL.Children.Add(info);
                }
                bullet.Children.Add(bulletL);
            }

            var bulletAMeta = data.Meta.aggregatableMetas[(int)MetaType.Bullet].GetAllFullWithDifficulty("All");
            if (bulletAMeta.Count > 0)
            {
                MetaModel bulletA = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "All"
                };
                foreach (MetaModel info in bulletAMeta)
                {
                    bulletA.Children.Add(info);
                }
                bullet.Children.Add(bulletA);
            }

            return bullet;
        }

        private MetaModel GetEditorObjMeta(MetaType type, string displayedName, string imgName)
        {
            MetaModel eobj = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/" + imgName,
                Text = displayedName
            };

            var eobjEMeta = data.Meta.aggregatableMetas[(int)type].GetAllFullWithDifficulty("Easy");
            if (eobjEMeta.Count > 0)
            {
                MetaModel eobjE = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Easy"
                };
                foreach (MetaModel info in eobjEMeta)
                {
                    eobjE.Children.Add(info);
                }
                eobj.Children.Add(eobjE);
            }

            var eobjNMeta = data.Meta.aggregatableMetas[(int)type].GetAllFullWithDifficulty("Normal");
            if (eobjNMeta.Count > 0)
            {
                MetaModel eobjN = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Normal"
                };
                foreach (MetaModel info in eobjNMeta)
                {
                    eobjN.Children.Add(info);
                }
                eobj.Children.Add(eobjN);
            }

            var eobjHMeta = data.Meta.aggregatableMetas[(int)type].GetAllFullWithDifficulty("Hard");
            if (eobjHMeta.Count > 0)
            {
                MetaModel eobjH = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Hard"
                };
                foreach (MetaModel info in eobjHMeta)
                {
                    eobjH.Children.Add(info);
                }
                eobj.Children.Add(eobjH);
            }

            var eobjLMeta = data.Meta.aggregatableMetas[(int)type].GetAllFullWithDifficulty("Lunatic");
            if (eobjLMeta.Count > 0)
            {
                MetaModel eobjL = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Lunatic"
                };
                foreach (MetaModel info in eobjLMeta)
                {
                    eobjL.Children.Add(info);
                }
                eobj.Children.Add(eobjL);
            }

            var eobjAMeta = data.Meta.aggregatableMetas[(int)type].GetAllFullWithDifficulty("All");
            if (eobjAMeta.Count > 0)
            {
                MetaModel eobjA = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "All"
                };
                foreach (MetaModel info in eobjAMeta)
                {
                    eobjA.Children.Add(info);
                }
                eobj.Children.Add(eobjA);
            }

            return eobj;
        }

        private MetaModel GetBentLaserMeta()
        {
            MetaModel bentlaser = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/laserbentdefine.png",
                Text = "Bent Laser"
            };

            var bentlaserEMeta = data.Meta.aggregatableMetas[(int)MetaType.BentLaser].GetAllFullWithDifficulty("Easy");
            if (bentlaserEMeta.Count > 0)
            {
                MetaModel bulletE = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Easy"
                };
                foreach (MetaModel info in bentlaserEMeta)
                {
                    bulletE.Children.Add(info);
                }
                bentlaser.Children.Add(bulletE);
            }

            var bentlaserNMeta = data.Meta.aggregatableMetas[(int)MetaType.BentLaser].GetAllFullWithDifficulty("Normal");
            if (bentlaserNMeta.Count > 0)
            {
                MetaModel bulletN = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Normal"
                };
                foreach (MetaModel info in bentlaserNMeta)
                {
                    bulletN.Children.Add(info);
                }
                bentlaser.Children.Add(bulletN);
            }

            var bentlaserHMeta = data.Meta.aggregatableMetas[(int)MetaType.BentLaser].GetAllFullWithDifficulty("Hard");
            if (bentlaserHMeta.Count > 0)
            {
                MetaModel bulletH = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Hard"
                };
                foreach (MetaModel info in bentlaserHMeta)
                {
                    bulletH.Children.Add(info);
                }
                bentlaser.Children.Add(bulletH);
            }

            var bentlaserLMeta = data.Meta.aggregatableMetas[(int)MetaType.BentLaser].GetAllFullWithDifficulty("Lunatic");
            if (bentlaserLMeta.Count > 0)
            {
                MetaModel bulletL = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Lunatic"
                };
                foreach (MetaModel info in bentlaserLMeta)
                {
                    bulletL.Children.Add(info);
                }
                bentlaser.Children.Add(bulletL);
            }

            var bentlaserAMeta = data.Meta.aggregatableMetas[(int)MetaType.BentLaser].GetAllFullWithDifficulty("All");
            if (bentlaserAMeta.Count > 0)
            {
                MetaModel bulletA = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "All"
                };
                foreach (MetaModel info in bentlaserAMeta)
                {
                    bulletA.Children.Add(info);
                }
                bentlaser.Children.Add(bulletA);
            }

            return bentlaser;
        }

        private MetaModel GetBossMeta()
        {
            MetaModel boss = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/bossdefine.png",
                Text = "Bosses"
            };

            var bossEMeta = data.Meta.aggregatableMetas[(int)MetaType.Boss].GetAllFullWithDifficulty("Easy");
            if (bossEMeta.Count > 0)
            {
                MetaModel bossE = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Easy"
                };
                foreach (MetaModel info in bossEMeta)
                {
                    bossE.Children.Add(info);
                }
                boss.Children.Add(bossE);
            }

            var bossNMeta = data.Meta.aggregatableMetas[(int)MetaType.Boss].GetAllFullWithDifficulty("Normal");
            if (bossNMeta.Count > 0)
            {
                MetaModel bossN = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Normal"
                };
                foreach (MetaModel info in bossNMeta)
                {
                    bossN.Children.Add(info);
                }
                boss.Children.Add(bossN);
            }

            var bossHMeta = data.Meta.aggregatableMetas[(int)MetaType.Boss].GetAllFullWithDifficulty("Hard");
            if (bossHMeta.Count > 0)
            {
                MetaModel bossH = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Hard"
                };
                foreach (MetaModel info in bossHMeta)
                {
                    bossH.Children.Add(info);
                }
                boss.Children.Add(bossH);
            }

            var bossLMeta = data.Meta.aggregatableMetas[(int)MetaType.Boss].GetAllFullWithDifficulty("Lunatic");
            if (bossLMeta.Count > 0)
            {
                MetaModel BossL = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "Lunatic"
                };
                foreach (MetaModel info in bossLMeta)
                {
                    BossL.Children.Add(info);
                }
                boss.Children.Add(BossL);
            }

            var bossAMeta = data.Meta.aggregatableMetas[(int)MetaType.Boss].GetAllFullWithDifficulty("All");
            if (bossAMeta.Count > 0)
            {
                MetaModel bossA = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/callbackfunc.png",
                    Text = "All"
                };
                foreach (MetaModel info in bossAMeta)
                {
                    bossA.Children.Add(info);
                }
                boss.Children.Add(bossA);
            }

            return boss;
        }

        private MetaModel GetImageMeta()
        {
            MetaModel images = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/loadimage.png",
                Text = "Images"
            };
            var a = data.Meta.aggregatableMetas[(int)MetaType.ImageLoad].GetAllFullWithDifficulty();
            foreach (MetaModel info in a)
            {
                images.Children.Add(info);
            }

            return images;
        }

        private MetaModel GetTaskMeta()
        {
            MetaModel tasks = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/taskdefine.png",
                Text = "Task"
            };
            var a = data.Meta.aggregatableMetas[(int)MetaType.Task].GetAllFullWithDifficulty();
            foreach (MetaModel info in a)
            {
                tasks.Children.Add(info);
            }

            return tasks;
        }

        private MetaModel GetImageGroupMeta()
        {
            MetaModel images = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/loadimagegroup.png",
                Text = "Image Group"
            };
            var a = data.Meta.aggregatableMetas[(int)MetaType.ImageGroupLoad].GetAllFullWithDifficulty();
            foreach (MetaModel info in a)
            {
                images.Children.Add(info);
            }

            return images;
        }

        private MetaModel GetBGMMeta()
        {
            MetaModel bgms = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/loadbgm.png",
                Text = "Musics"
            };
            var a = data.Meta.aggregatableMetas[(int)MetaType.BGMLoad].GetAllFullWithDifficulty();
            foreach (MetaModel info in a)
            {
                bgms.Children.Add(info);
            }

            return bgms;
        }

        private MetaModel GetBossBGDefineMeta()
        {
            MetaModel bossBG = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/bgdefine.png",
                Text = "Boss Backgrounds"
            };
            var a = data.Meta.aggregatableMetas[(int)MetaType.BossBG].GetAllFullWithDifficulty();
            foreach (MetaModel info in a)
            {
                bossBG.Children.Add(info);
            }

            return bossBG;
        }

        public ViewDefinition(DocumentData data)
        {
            this.data = data;
            InitializeComponent();
            InitializeTree();
            AllDef.ItemsSource = Tree;
        }
    }
}
