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
    public enum ImageClassType 
    {
        None = 0,
        Animation = 1,
        Particle = 2,
        All = 3
    }

    public class InputWindowSelectorRegister : IInputWindowSelectorRegister
    {
        public void RegisterComboBoxText(Dictionary<string, string[]> target)
        {
            target.Add("bool"
                , new string[] { "true", "false" });
            target.Add("sineinterpolation"
                , new string[] { "SINE_ACCEL", "SINE_DECEL", "SINE_ACC_DEC" });
            target.Add("target"
                , new string[] { "self", "last", "unit", "player", "_boss" });
            target.Add("yield"
                , new string[] { "_infinite" });
            target.Add("nullabletarget"
                , new string[] { "", "self", "last", "unit", "player", "_boss" });
            target.Add("blend"
                , new string[] { "\"\"", "\"mul+add\"", "\"mul+alpha\"", "\"add+add\"", "\"add+alpha\""
                        , "\"mul+sub\"", "\"mul+rev\"", "\"add+sub\"", "\"add+rev\"", "\"alpha+bal\"" });
            target.Add("event"
                , new string[] { "frame", "kill", "del", "colli" });//, "render" });
            target.Add("interpolation"
                , new string[] { "MOVE_NORMAL", "MOVE_ACCEL", "MOVE_DECEL", "MOVE_ACC_DEC" });
            target.Add("modification"
                , new string[] { "MODE_SET", "MODE_ADD", "MODE_MUL" });
            target.Add("group"
                , new string[] { "GROUP_GHOST", "GROUP_ENEMY_BULLET", "GROUP_ENEMY", "GROUP_PLAYER_BULLET"
                        , "GROUP_PLAYER", "GROUP_INDES", "GROUP_ITEM", "GROUP_NONTJT" });
            target.Add("layer"
                , new string[] { "LAYER_BG-5", "LAYER_BG", "LAYER_BG+5", "LAYER_ENEMY-5", "LAYER_ENEMY"
                        , "LAYER_ENEMY+5", "LAYER_PLAYER_BULLET-5", "LAYER_PLAYER_BULLET", "LAYER_PLAYER_BULLET+5"
                        , "LAYER_PLAYER-5", "LAYER_PLAYER", "LAYER_PLAYER+5", "LAYER_ITEM-5", "LAYER_ITEM"
                        , "LAYER_ITEM+5", "LAYER_ENEMY_BULLET-5", "LAYER_ENEMY_BULLET", "LAYER_ENEMY_BULLET+5"
                        , "LAYER_ENEMY_BULLET_EF-5", "LAYER_ENEMY_BULLET_EF", "LAYER_ENEMY_BULLET_EF+5"
                        , "LAYER_TOP-5", "LAYER_TOP", "LAYER_TOP+5" });
            target.Add("stageGroup"
                , new string[] { "Easy", "Normal", "Hard", "Lunatic", "Extra" });
            target.Add("objDifficulty"
                , new string[] { "All", "Easy", "Normal", "Hard", "Lunatic" });
            target.Add("difficulty"
                , new string[] { "1", "2", "3", "4", "5" });
            target.Add("SCName"
                , new string[] { "", "「」" });
            target.Add("bulletStyle"
                , new string[] { "arrow_big", "arrow_mid", "arrow_small", "gun_bullet", "butterfly"
                        , "square", "ball_small", "ball_mid", "ball_mid_c", "ball_big", "ball_huge"
                        , "ball_light", "star_small", "star_big", "grain_a", "grain_b", "grain_c", "kite"
                        , "knife", "knife_b", "water_drop", "mildew", "ellipse", "heart", "money", "music"
                        , "silence", "water_drop_dark", "ball_huge_dark", "ball_light_dark"});
            target.Add("laserStyle"
                , new string[] { "1", "2", "3", "4" });
            target.Add("color"
                , new string[] { "COLOR_RED", "COLOR_DEEP_RED", "COLOR_PURPLE", "COLOR_DEEP_PURPLE"
                        , "COLOR_BLUE", "COLOR_DEEP_BLUE", "COLOR_ROYAL_BLUE", "COLOR_CYAN", "COLOR_DEEP_GREEN"
                        , "COLOR_GREEN", "COLOR_CHARTREUSE", "COLOR_YELLOW", "COLOR_GOLDEN_YELLOW", "COLOR_ORANGE"
                        , "COLOR_DEEP_GRAY", "COLOR_GRAY"});
            target.Add("nullableColor"
                , new string[] { "", "COLOR_RED", "COLOR_DEEP_RED", "COLOR_PURPLE", "COLOR_DEEP_PURPLE"
                        , "COLOR_BLUE", "COLOR_DEEP_BLUE", "COLOR_ROYAL_BLUE", "COLOR_CYAN", "COLOR_DEEP_GREEN"
                        , "COLOR_GREEN", "COLOR_CHARTREUSE", "COLOR_YELLOW", "COLOR_GOLDEN_YELLOW", "COLOR_ORANGE"
                        , "COLOR_DEEP_GRAY", "COLOR_GRAY"});
            target.Add("objimage"
                , new string[] { "\"img_void\"", "\"white\"", "\"leaf\"" });
            target.Add("image"
                , new string[] { "\"img_void\"", "\"white\"", "\"leaf\"" });
            target.Add("BG"
                , new string[] { "temple_background", "magic_forest_background", "bamboo_background"
                        , "bamboo2_background", "cube_background", "gensokyosora_background", "hongmoguanB_background"
                        , "icepool_background", "lake_background", "le03_5_background", "magic_forest_fast_background"
                        , "river_background", "starlight_background", "temple2_background", "woods_background"
                        , "world_background"});
            target.Add("prop"
                , new string[] { "x", "y", "rot", "omiga", "timer", "vx", "vy", "ax", "ay", "layer", "group"
                        , "hide", "bound", "navi", "colli", "status", "hscale", "vscale", "a", "b", "rect", "img"
                        , "pause", "rmove", "nopause", "_angle", "_speed" });
            target.Add("valprop"
                , new string[] { "x", "y", "rot", "omiga", "timer", "vx", "vy", "ax", "ay", "layer", "group"
                        , "hscale", "vscale", "a", "b", "_angle", "_speed" });
            target.Add("se"
                , new string[] { "alert", "astralup", "bonus", "bonus2", "boon00", "boon01", "cancel00"
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
                        , "focusfix", "focusfix2", "focusin", "heal", "ice", "ice2", "item01", "ophide", "opshow" });
            target.Add("item"
                , new string[] { "item_power","item_faith","item_point","item_power_large","item_power_full"
                        ,"item_faith_minor", "item_extend","item_chip","item_bomb","item_bombchip"});
            target.Add("lrstr"
                , new string[] { "\"left\"", "\"right\"" });
            target.Add("directionMode"
                , new string[] { "MOVE_X_TOWARDS_PLAYER", "MOVE_Y_TOWARDS_PLAYER", "MOVE_TOWARDS_PLAYER", "MOVE_RANDOM" });
            target.Add("curve"
                , new string[] { "Bezier", "CR", "Basis2" });
            target.Add("renderOp"
                , new string[] { "Push", "Pop" });
            target.Add("curveRepeatType"
                , new string[] { "None", "Round Robin" });
        }

        public void RegisterInputWindow(Dictionary<string, Func<AttrItem, string, IInputWindow>> target)
        {
            target.Add("bool", (src, tar) => new Selector(tar, InputWindowSelector.SelectComboBox("bool"), "Input Bool"));
            target.Add("sineinterpolation", (source, toEdit) => new Selector(toEdit
                , InputWindowSelector.SelectComboBox("sineinterpolation"), "Input Sine Interpolation Mode"));
            target.Add("code", (src, tar) => new CodeInput(tar));
            target.Add("position", (src, tar) => new PositionInput(tar));
            target.Add("pointSet", (src, tar) => new PointSetInput(tar));
            target.Add("target", (src, tar) => new Selector(tar, InputWindowSelector.SelectComboBox("target"), "Input Target Object"));
            target.Add("imageFile", (src, tar) => new PathInput(tar, "Image File (*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp", src));
            target.Add("particleFile", (src, tar) => new PathInput(tar, "HGE Particle File (*.psi)|*.psi", src));
            target.Add("fxFile", (src, tar) => new PathInput(tar, "Shader File (*.fx)|*.fx", src));
            target.Add("audioFile", (src, tar) => new PathInput(tar, "Audio File (*.wav;*.ogg)|*.wav;*.ogg", src));
            target.Add("seFile", (src, tar) => new PathInput(tar, "Sound Effect File (*.wav;*.ogg)|*.wav;*.ogg", src));
            target.Add("luaFile", (src, tar) => new PathInput(tar, "Lua File (*.lua)|*.lua", src));
            target.Add("lstgesFile", (src, tar) => new PathInput(tar, "LuaSTG Sharp File (*.lstges)|*.lstges", src));
            target.Add("plainFile", (src, tar) => new PathInput(tar, "File (*.*)|*.*", src));
            target.Add("plainMultipleFiles", (src, tar) => new MultiplePathInput(tar, "File (*.*)|*.*", src));
            target.Add("SCName", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("SCName"), "Input Spell Card Name"));
            target.Add("blend", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("blend"), "Input Blend Mode Type"));
            target.Add("event", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("event"), "Input Event Type"));
            target.Add("interpolation", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("interpolation"), "Input Interpolation Type"));
            target.Add("modification", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("modification"), "Input Modification Type"));
            target.Add("group", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("group"), "Input Group Type"));
            target.Add("layer", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("layer"), "Input Layer"));
            target.Add("stageGroup", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("stageGroup"), "Input Stage Group"));
            target.Add("objDifficulty", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("objDifficulty"), "Input Difficulty"));
            target.Add("difficulty", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("difficulty"), "Input Difficulty Value"));
            target.Add("prop", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("prop"), "Input Properties"));
            target.Add("directionMode", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("directionMode"), "Input Direction Mode"));
            target.Add("curve", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("curve"), "Input Curve Type"));
            target.Add("renderOp", (src, tar) => new Selector(tar
                , InputWindowSelector.SelectComboBox("renderOp"), "Input Render Target Operation"));
            target.Add("bulletStyle", (src, tar) => new BulletInput(tar));
            target.Add("laserStyle", (src, tar) => new LaserInput(tar));
            target.Add("enemyStyle", (src, tar) => new EnemyInput(tar));
            target.Add("userDefinedNodeDefinition", (src, tar) => new NodeDefInput(tar, src));
            target.Add("bulletDef", (src, tar) => new EditorObjDefInput(tar, MetaType.Bullet, src));
            target.Add("objectDef", (src, tar) => new EditorObjDefInput(tar, MetaType.Object, src));
            target.Add("laserDef", (src, tar) => new EditorObjDefInput(tar, MetaType.Laser, src));
            target.Add("bentLaserDef", (src, tar) => new EditorObjDefInput(tar, MetaType.BentLaser, src));
            target.Add("enemyDef", (src, tar) => new EditorObjDefInput(tar, MetaType.Enemy, src));
            target.Add("taskDef", (src, tar) => new EditorObjDefInput(tar, MetaType.Task, src));
            target.Add("bossDef", (src, tar) => new EditorObjDefInput(tar, MetaType.Boss, src));
            target.Add("objimage", (src, tar) => new ImageInput(tar, src, ImageClassType.Animation | ImageClassType.Particle));
            target.Add("image", (src, tar) => new ImageInput(tar, src));
            target.Add("BGM", (src, tar) => new BGMInput(tar, src));
            target.Add("se", (src, tar) => new SEInput(tar, src));
            target.Add("fx", (src, tar) => new FXInput(tar, src));
            target.Add("multilineText", (src, tar) => new MultilineInput(tar));
            target.Add("bulletParam", (src, tar) => new EditorObjParamInput(src, MetaType.Bullet, tar));
            target.Add("objectParam", (src, tar) => new EditorObjParamInput(src, MetaType.Object, tar));
            target.Add("laserParam", (src, tar) => new EditorObjParamInput(src, MetaType.Laser, tar));
            target.Add("bentLaserParam", (src, tar) => new EditorObjParamInput(src, MetaType.BentLaser, tar));
            target.Add("enemyParam", (src, tar) => new EditorObjParamInput(src, MetaType.Enemy, tar));
            target.Add("taskParam", (src, tar) => new EditorObjParamInput(src, MetaType.Task, tar));
            target.Add("color", (src, tar) => new ColorInput(tar));
            target.Add("nullableColor", (src, tar) => new ColorInput(tar));
            target.Add("ARGB", (src, tar) => new ARGBInput(tar));
            target.Add("RGB", (src, tar) => new ARGBInput(tar, false));
            target.Add("vector", (src, tar) => new VectorInput(tar));
            target.Add("size", (src, tar) => new SizeInput(tar));
            target.Add("bossBG", (src, tar) => new BossBGDefInput(tar, src));
            //GUGUGU
            target.Add("scale", InputWindowSelector.nullWindow);
            target.Add("colrow", InputWindowSelector.nullWindow);
            target.Add("velocity", InputWindowSelector.nullWindow);
            target.Add("velocityPos", InputWindowSelector.nullWindow);
            target.Add("rotation", InputWindowSelector.nullWindow);
            target.Add("animinterval", InputWindowSelector.nullWindow);
            target.Add("rect", InputWindowSelector.nullWindow);
            target.Add("rectNonNegative", InputWindowSelector.nullWindow);
            target.Add("omega", InputWindowSelector.nullWindow);
        }
    }
}
