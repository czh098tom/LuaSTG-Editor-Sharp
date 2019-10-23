using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.Plugin;

namespace LuaSTGEditorSharp.Windows.Input
{
    public class InputWindowSelector : IInputWindowSelector
    {
        public string[] SelectComboBox(string name)
        {
            switch (name)
            {
                case "bool":
                    return new string[] { "true", "false" };
                case "target":
                    return new string[] { "self", "last", "unit", "player", "_boss" };
                case "yield":
                    return new string[] { "_infinite" };
                case "nullabletarget":
                    return new string[] { "", "self", "last", "unit", "player", "_boss" };
                case "blend":
                    return new string[] { "\"\"", "\"mul+add\"", "\"mul+alpha\"", "\"add+add\"", "\"add+alpha\""
                        , "\"mul+sub\"", "\"mul+rev\"", "\"add+sub\"", "\"add+rev\"", "\"alpha+bal\"" };
                case "event":
                    return new string[] { "frame", "kill", "del", "colli" };//, "render" };
                case "interpolation":
                    return new string[] { "MOVE_NORMAL", "MOVE_ACCEL", "MOVE_DECEL", "MOVE_ACC_DEC" };
                case "modification":
                    return new string[] { "MODE_SET", "MODE_ADD", "MODE_MUL" };
                case "group":
                    return new string[] { "GROUP_GHOST", "GROUP_ENEMY_BULLET", "GROUP_ENEMY", "GROUP_PLAYER_BULLET"
                        , "GROUP_PLAYER", "GROUP_INDES", "GROUP_ITEM", "GROUP_NONTJT" };
                case "layer":
                    return new string[] { "LAYER_BG-5", "LAYER_BG", "LAYER_BG+5", "LAYER_ENEMY-5", "LAYER_ENEMY"
                        , "LAYER_ENEMY+5", "LAYER_PLAYER_BULLET-5", "LAYER_PLAYER_BULLET", "LAYER_PLAYER_BULLET+5"
                        , "LAYER_PLAYER-5", "LAYER_PLAYER", "LAYER_PLAYER+5", "LAYER_ITEM-5", "LAYER_ITEM"
                        , "LAYER_ITEM+5", "LAYER_ENEMY_BULLET-5", "LAYER_ENEMY_BULLET", "LAYER_ENEMY_BULLET+5"
                        , "LAYER_ENEMY_BULLET_EF-5", "LAYER_ENEMY_BULLET_EF", "LAYER_ENEMY_BULLET_EF+5"
                        , "LAYER_TOP-5", "LAYER_TOP", "LAYER_TOP+5" };
                case "stageGroup":
                    return new string[] { "Easy", "Normal", "Hard", "Lunatic", "Extra" };
                case "objDifficulty":
                    return new string[] { "All", "Easy", "Normal", "Hard", "Lunatic" };
                case "difficulty":
                    return new string[] { "1", "2", "3", "4", "5" };
                case "SCName":
                    return new string[] { "", "「」" };
                case "bulletStyle":
                    return new string[] { "arrow_big", "arrow_mid", "arrow_small", "gun_bullet", "butterfly"
                        , "square", "ball_small", "ball_mid", "ball_mid_c", "ball_big", "ball_huge"
                        , "ball_light", "star_small", "star_big", "grain_a", "grain_b", "grain_c", "kite"
                        , "knife", "knife_b", "water_drop", "mildew", "ellipse", "heart", "money", "music"
                        , "silence", "water_drop_dark", "ball_huge_dark", "ball_light_dark"};
                case "laserStyle":
                    return new string[] { "1", "2", "3", "4" };
                case "color":
                    return new string[] { "COLOR_RED", "COLOR_DEEP_RED", "COLOR_PURPLE", "COLOR_DEEP_PURPLE"
                        , "COLOR_BLUE", "COLOR_DEEP_BLUE", "COLOR_ROYAL_BLUE", "COLOR_CYAN", "COLOR_DEEP_GREEN"
                        , "COLOR_GREEN", "COLOR_CHARTREUSE", "COLOR_YELLOW", "COLOR_GOLDEN_YELLOW", "COLOR_ORANGE"
                        , "COLOR_DEEP_GRAY", "COLOR_GRAY"};
                case "nullableColor":
                    return new string[] { "", "COLOR_RED", "COLOR_DEEP_RED", "COLOR_PURPLE", "COLOR_DEEP_PURPLE"
                        , "COLOR_BLUE", "COLOR_DEEP_BLUE", "COLOR_ROYAL_BLUE", "COLOR_CYAN", "COLOR_DEEP_GREEN"
                        , "COLOR_GREEN", "COLOR_CHARTREUSE", "COLOR_YELLOW", "COLOR_GOLDEN_YELLOW", "COLOR_ORANGE"
                        , "COLOR_DEEP_GRAY", "COLOR_GRAY"};
                case "image":
                    return new string[] { "\"img_void\"", "\"white\"", "\"leaf\"" };
                case "BG":
                    return new string[] { "temple_background", "magic_forest_background", "bamboo_background"
                        , "bamboo2_background", "cube_background", "gensokyosora_background", "hongmoguanB_background"
                        , "icepool_background", "lake_background", "le03_5_background", "magic_forest_fast_background"
                        , "river_background", "starlight_background", "temple2_background", "woods_background"
                        , "world_background"};
                case "prop":
                    return new string[] { "x", "y", "rot", "omiga", "timer", "vx", "vy", "ax", "ay", "layer", "group"
                        , "hide", "bound", "navi", "colli", "status", "hscale", "vscale", "a", "b", "rect", "img"
                        , "pause", "rmove", "nopause", "_angle", "_speed" };
                case "se":
                    return new string[] { "alert", "astralup", "bonus", "bonus2", "boon00", "boon01", "cancel00"
                        , "cardget", "cat00", "cat01", "ch00", "ch01", "ch02", "don00", "damage00", "damage01"
                        , "enep00", "enep01", "enep02", "extend", "fault", "graze", "gun00", "hint00", "invalid"
                        , "item00", "kira00", "kira01", "kira02", "lazer00", "lazer01", "lazer02", "msl", "msl2"
                        , "nep00", "ok00", "option", "pause", "pldead00", "plst00", "power0", "power1", "powerup"
                        , "select00", "slash", "tan00", "tan01", "tan02", "timeout", "timeout2","warpl", "warpr"
                        , "water", "explode", "nice", "nodamage", "power02", "lgods1", "lgods2", "lgods3", "lgods4"
                        , "lgodsget", "big", "wolf", "noise", "pin00", "powerup1", "old_cat00", "old_enep00"
                        , "old_extend", "old_gun00", "old_kira00", "old_kira01", "old_lazer01", "old_nep00"
                        , "old_pldead00", "old_power0", "old_power1", "old_powerup", "hyz_charge00", "hyz_charge01b"
                        , "hyz_chargeup", "hyz_eterase", "hyz_exattack", "hyz_gosp", "hyz_life1", "hyz_playerdead"
                        , "hyz_timestop0", "hyz_warning", "bonus3", "border", "changeitem", "down", "extend2"
                        , "focusfix", "focusfix2", "focusin", "heal", "ice", "ice2", "item01", "ophide", "opshow" };
                case "lrstr":
                    return new string[] { "\"left\"", "\"right\"" };
                case "directionMode":
                    return new string[] { "MOVE_X_TOWARDS_PLAYER", "MOVE_Y_TOWARDS_PLAYER", "MOVE_TOWARDS_PLAYER", "MOVE_RANDOM" };
                default:
                    return new string[] { };
            }
        }

        public InputWindow SelectInputWindow(AttrItem source, string name, string toEdit, MainWindow owner)
        {
            InputWindow window;
            switch (name)
            {
                case "bool":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Bool");
                    break;
                case "code":
                    window = new CodeInput(toEdit, owner);
                    break;
                case "position":
                    window = new PositionInput(toEdit, owner);
                    break;
                case "target":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Target Object");
                    break;
                case "imageFile":
                    window = new PathInput(toEdit, "Image File (*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp", owner);
                    break;
                case "audioFile":
                    window = new PathInput(toEdit, "Audio File (*.wav;*.ogg)|*.wav;*.ogg", owner);
                    break;
                case "seFile":
                    window = new PathInput(toEdit, "Sound Effect File (*.wav;*.ogg)|*.wav;*.ogg", owner);
                    break;
                case "luaFile":
                    window = new PathInput(toEdit, "Lua File (*.lua)|*.lua", owner);
                    break;
                case "lstgesFile":
                    window = new PathInput(toEdit, "LuaSTG Sharp File (*.lstges)|*.lstges", owner);
                    break;
                case "SCName":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Spell Card Name");
                    break;
                case "blend":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Blend Mode Type");
                    break;
                case "event":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Event Type");
                    break;
                case "interpolation":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Interpolation Type");
                    break;
                case "modification":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Modification Type");
                    break;
                case "group":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Group Type");
                    break;
                case "layer":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Layer");
                    break;
                case "stageGroup":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Stage Group");
                    break;
                case "objDifficulty":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Difficulty");
                    break;
                case "difficulty":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Difficulty Value");
                    break;
                case "prop":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Properties");
                    break;
                case "directionMode":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Direction Mode");
                    break;
                case "bulletStyle":
                    window = new BulletInput(toEdit, owner);
                    break;
                case "laserStyle":
                    window = new LaserInput(toEdit, owner);
                    break;
                case "enemyStyle":
                    window = new EnemyInput(toEdit, owner);
                    break;
                case "userDefinedNode":
                    window = new NodeDefInput(toEdit, owner, source);
                    break;
                case "bulletDef":
                    window = new EditorObjDefInput(toEdit, MetaType.Bullet, owner, source);
                    break;
                case "objectDef":
                    window = new EditorObjDefInput(toEdit, MetaType.Object, owner, source);
                    break;
                case "laserDef":
                    window = new EditorObjDefInput(toEdit, MetaType.Laser, owner, source);
                    break;
                case "bentLaserDef":
                    window = new EditorObjDefInput(toEdit, MetaType.BentLaser, owner, source);
                    break;
                case "enemyDef":
                    window = new EditorObjDefInput(toEdit, MetaType.Enemy, owner, source);
                    break;
                case "taskDef":
                    window = new EditorObjDefInput(toEdit, MetaType.Task, owner, source);
                    break;
                case "bossDef":
                    window = new EditorObjDefInput(toEdit, MetaType.Boss, owner, source);//new BossDefInput(toEdit, owner, source);
                    break;
                case "image":
                    window = new ImageInput(toEdit, owner, source);
                    break;
                case "BGM":
                    window = new BGMInput(toEdit, owner, source);
                    break;
                case "se":
                    window = new SEInput(toEdit, owner, source);
                    break;
                case "multilineText":
                    window = new MultilineInput(toEdit);
                    break;
                case "bulletParam":
                    window = new EditorObjParamInput(source, MetaType.Bullet, toEdit, owner);
                    break;
                case "objectParam":
                    window = new EditorObjParamInput(source, MetaType.Object, toEdit, owner);
                    break;
                case "laserParam":
                    window = new EditorObjParamInput(source, MetaType.Laser, toEdit, owner);
                    break;
                case "bentLaserParam":
                    window = new EditorObjParamInput(source, MetaType.BentLaser, toEdit, owner);
                    break;
                case "enemyParam":
                    window = new EditorObjParamInput(source, MetaType.Enemy, toEdit, owner);
                    break;
                case "taskParam":
                    window = new EditorObjParamInput(source, MetaType.Task, toEdit, owner);
                    break;
                case "color":
                    window = new ColorInput(toEdit, owner);
                    break;
                case "nullableColor":
                    window = new ColorInput(toEdit, owner);
                    break;
                case "ARGB":
                    window = new ARGBInput(toEdit, owner);
                    break;
                case "vector":
                    window = new VectorInput(toEdit, owner);
                    break;
                case "size":
                    window = new SizeInput(toEdit, owner);
                    break;
                case "scale":
                case "bossBG":
                    window = new BossBGDefInput(toEdit, owner, source);
                    break;
                //GUGUGU
                case "colrow":
                case "velocity":
                case "velocityPos":
                case "rotation":
                case "animinterval":
                case "rect":
                case "rectNonNegative":
                case "omega":
                default:
                    window = new SingleLineInput(toEdit, owner);
                    break;
            }
            window.AppendTitle(source.AttrCap);
            return window;
        }
    }
}
