using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System.Collections;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    using Skills = Images.Monster.Skill;

    public enum MonsterState { Alive, Death }
    public enum MoveState { Walking, Running, Attacking1, Attacking2, Standing, Death1, Death2, Fade1, Fade2, AttackingRunning, FreezeStart, Freeze, Stun, 
        Cast, Custom1 }
    public enum MonsterAbility { None = 0, EnergySheild = 1, ManaShield = 2 }
    public struct MonsterStats {
        public float Life_Maximum, Life_Regeneration, Life_Regeneration_Percent;
        public float Mana_Maximum, Mana_Regeneration, Mana_Regeneration_Percent;
        public float[] Attack_Physical_Damage, Attack_Fire_Damage, Attack_Cold_Damage, Attack_Lightning_Damage, Attack_Chaos_Damage;
        public string[] Abilities;
        public float Armor, Fire_Resistance, Cold_Resistance, Lightning_Resistance, Chaos_Resistance;
        public float Block_Damage, Block_Chance;
        public float Critical_Strike_Chance, Critical_Strike_Multiplier;
        public float Movement_Speed_Walking, Movement_Speed_Running;
        public float Attack_Cooldown_Time, Attack_Range;
        //Bounty
        public float Gold_Bounty, XP_Bounty;
        
        public static MonsterStats Zero;
        static MonsterStats() {
            Array.Resize(ref Zero.Attack_Physical_Damage, 2);
            Array.Resize(ref Zero.Attack_Fire_Damage, 2);
            Array.Resize(ref Zero.Attack_Cold_Damage, 2);
            Array.Resize(ref Zero.Attack_Lightning_Damage, 2);
            Array.Resize(ref Zero.Attack_Chaos_Damage, 2);
        }
    }
    public abstract partial class Monster : GameObject {
        public float Life, Mana;
        //increase % 
        public float Increased_Movement_Speed;
        public float Increased_Attack_Speed;

        public AuraCollection Auras = new AuraCollection();
        public AuraCollection EffectedAuras = new AuraCollection();

        public MonsterStats Stats;
        public MonsterState State;
        public MoveState Animation_State;

        //Elemental status ailments

        public static float Base_Stun_Duration = 0.35f;
        public static float Base_Ignite_Duration = 4f;
        public static float Base_Freeze_Duration = 10f;
        public static float Base_Chill_Duration = 13.8f;
        public static float Base_Shock_Duration = 27.6f;
        public static float Capped_Duration = 0.3f;

        public bool Stun, Ignite, Chill, Freeze, Shock;
        public float Stun_Duration, Ignite_Duration, Chill_Duration, Freeze_Duration, Shock_Duration;
        public float Ignite_DPS;
        
        //Knock Back
        public static float Base_Knockback_Duration = 1;
        public float Knockback_Duration, Knockback_Speed, Acceleration;

        //Tag
        public float Float;
        //Graphic
        public Images.Monster.ArtModelMonster ArtMonster;
        /// <summary> Rectangle base on x,y. Đây là Rectangle trục tọa độ từ x,y </summary>
        public Rectangle Collision_Rect = new Rectangle(-15, -35, 35, 60); 

        private float Distance(float x1, float y1, float x2, float y2) {
            return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
    public abstract partial class Monster {        
        ///<summary>Monster.DrawOnTop sẻ vẻ HP Bar.</summary>
        public override void DrawOnTop2() {
            if (State == MonsterState.Alive) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
                Images.UI.MonsterHPBar.Draw(new Vector2(Position.X, Position.Y + Collision_Rect.Y - 8), Life / Stats.Life_Maximum);
                e.SpriteBatch.End();
            }
        }
        #region Macros
        public void RegenLife(float life) { 
            Life += life; if (Life >= Stats.Life_Maximum) { Life = Stats.Life_Maximum; }
            else if (Life < 0) {
                Life = 0;
                if (Animation_State == MoveState.Walking) { Vx = -Stats.Movement_Speed_Walking *2 / GameSetting.SecondPerFrame; }
                else if (Animation_State == MoveState.Running) { Vx = -Stats.Movement_Speed_Running*2 / GameSetting.SecondPerFrame; }
                SetMoveState(MoveState.Death2); State = MonsterState.Death;
            }
        }
        public void RegenMana(float mana) { Mana += mana; if (Mana >= Stats.Mana_Maximum) { Mana = Stats.Mana_Maximum; } else if (Mana < 0) { Mana = 0; } }
        public float Vx = -0, Vy = 0, Opacity = 1, OpacityTime;
        public void DoDeath(float HitX, float HitY) {
            double Alpha = Position.GetPointRotate(new Vector2(HitX, HitY));
            Vx = (float)Math.Cos(Alpha) * Base_Death_Distance; Vy = (float)Math.Sin(Alpha) * Base_Death_Distance;
            State = MonsterState.Death;
            if (Vx >= 0) { SetMoveState(MoveState.Death1); }
            else { SetMoveState(MoveState.Death2); }
        }
        public void SetMoveState(MoveState e) {
            Animation_State = e; 
            for (int i = 0; i < ArtMonster.Frames.Length; i++) {
                if (ArtMonster.Frames[i].State == Animation_State) {
                    FrameIndex = i;
                    break;
                }
            }
        }
        #endregion


        public virtual void OnDeath() { e.Player.ReceiveXP(this.Stats.XP_Bounty); e.Player.OnKilledMonster(this); }
    }
    public abstract partial class Monster {
        public static float Base_Death_Distance = 200;
        public static float Base_Fade_Ticks = 260;
        public float TimeAttackRemain; 
        public int FrameIndex, Waited_Ticks;

        public override void Update() {
            if (State == MonsterState.Alive) {
                RegenLife((Stats.Life_Maximum * Stats.Life_Regeneration_Percent + Stats.Life_Regeneration) * GameSetting.SecondPerFrame);
                RegenMana((Stats.Mana_Maximum * Stats.Mana_Regeneration_Percent + Stats.Mana_Regeneration) * GameSetting.SecondPerFrame);
                for (int i = 0; i < Auras.Count; i++) { Auras.Items[i].Update(); }

                float Speed_Multiplier = 1;

                if (Knockback_Duration > 0) {
                    Knockback_Duration -= GameSetting.SecondPerFrame;
                    Position.X = Position.X + Knockback_Speed * GameSetting.SecondPerFrame;
                    Knockback_Speed -= Acceleration * GameSetting.SecondPerFrame;
                }

                if (Shock == true) {
                    Shock_Duration -= GameSetting.SecondPerFrame;
                    if (Shock_Duration <= 0) { Shock_Duration = 0; Shock = false; }
                }

                if (Ignite == true) {
                    TakeDamageOverTime(0, Ignite_DPS * GameSetting.SecondPerFrame, 0, 0, 0);
                    Ignite_Duration -= GameSetting.SecondPerFrame;
                    if (Ignite_Duration <= 0) { Ignite_Duration = 0; Ignite = false; }
                }

                if (Chill == true) {
                    Chill_Duration -= GameSetting.SecondPerFrame;
                    if (Chill_Duration <= 0) { Chill_Duration = 0; Chill = false; }
                    Speed_Multiplier = 0.7f;
                }

                if (Stun == true) {
                    Stun_Duration -= GameSetting.SecondPerFrame;
                    if (Stun_Duration <= 0) { Stun = false; SetMoveState(MoveState.Walking); }
                }

                if (Freeze == true) {
                    Freeze_Duration -= GameSetting.SecondPerFrame;
                    if (Freeze_Duration <= 0) { Freeze_Duration = 0; Freeze = false; SetMoveState(MoveState.Walking); }
                    Speed_Multiplier = 0;
                }

                Waited_Ticks += 1;
                if (Waited_Ticks >= ArtMonster.Frames[FrameIndex].wait) { OnNextFrame(); }
                if (Freeze == true || Stun == true) { return; }

                TimeAttackRemain -= GameSetting.SecondPerFrame;
                if (ArtMonster.Frames[FrameIndex].IsAttack == true && TimeAttackRemain <= 0) {
                    TimeAttackRemain = Stats.Attack_Cooldown_Time;
                    if (Position.X <= Stats.Attack_Range) { OnAttack(); }
                }



                if (Animation_State == MoveState.Walking) {
                    Position.X -= this.Stats.Movement_Speed_Walking * (1 + this.Increased_Movement_Speed) * Speed_Multiplier;
                    if (Position.X <= Stats.Attack_Range) {
                        if (TimeAttackRemain <= 0) { DoAttack(); }
                        else { SetMoveState(MoveState.Standing); }
                    }
                } else if (Animation_State == MoveState.Running) {
                    Position.X -= this.Stats.Movement_Speed_Running * (1 + this.Increased_Movement_Speed) * Speed_Multiplier;
                    if (Position.X <= Stats.Attack_Range) { DoAttack(); }
                } else if (Animation_State == MoveState.Standing) {
                    if (Position.X <= Stats.Attack_Range) { if (TimeAttackRemain <= 0) { DoAttack(); } }
                    else { SetMoveState(MoveState.Walking); }
                }
            } 
            else if (State == MonsterState.Death) { 
                Waited_Ticks += 1; TimeAttackRemain -= GameSetting.SecondPerFrame;
                if (Waited_Ticks >= ArtMonster.Frames[FrameIndex].wait) {
                    Waited_Ticks = 0; FrameIndex = ArtMonster.Frames[FrameIndex].next;
                    Animation_State = ArtMonster.Frames[FrameIndex].State;
                    //Animation_State = (MoveState)Enum.Parse(typeof(MoveState), ArtMonster.Frames[FrameIndex].name);
                }
                if (Animation_State == MoveState.Death1 || Animation_State == MoveState.Death2) {
                    Position.Y += Vy * InfoSystemGame.SecondPerFrame - 5 * InfoSystemGame.SecondPerFrame * InfoSystemGame.SecondPerFrame;
                    Position.X += this.Vx * InfoSystemGame.SecondPerFrame;
                }
                
                if (Animation_State == MoveState.Fade1 || Animation_State == MoveState.Fade2) {
                    OpacityTime += 1;
                    if (OpacityTime <= Base_Fade_Ticks) {
                        Opacity = 1 - Waited_Ticks / Base_Fade_Ticks;
                    } else {
                        Deletable = true;
                    };
                }
            }
        }
        public virtual void DoAttack() {
            if (Animation_State == MoveState.Running) { SetMoveState(MoveState.AttackingRunning); }
            else if (Animation_State == MoveState.Attacking1 || Animation_State == MoveState.Attacking2) { }
            else {
                int i = GameSetting.RND.Next(2);
                if (i == 1) { SetMoveState(MoveState.Attacking1); }
                else { SetMoveState(MoveState.Attacking2); }
            }    
        }
        public virtual void OnAttack() {
            e.Player.TakeDamage(this,
                GameSetting.RND.NextFloat(Stats.Attack_Physical_Damage[0], Stats.Attack_Physical_Damage[1]),
                GameSetting.RND.NextFloat(Stats.Attack_Fire_Damage[0], Stats.Attack_Fire_Damage[1]),
                GameSetting.RND.NextFloat(Stats.Attack_Cold_Damage[0], Stats.Attack_Cold_Damage[1]),
                GameSetting.RND.NextFloat(Stats.Attack_Lightning_Damage[0], Stats.Attack_Lightning_Damage[1]),
                GameSetting.RND.NextFloat(Stats.Attack_Chaos_Damage[0], Stats.Attack_Chaos_Damage[1]),
                Stats.Critical_Strike_Chance, Stats.Critical_Strike_Multiplier);
        }
        public virtual void OnNextFrame() {
            FrameIndex = ArtMonster.Frames[FrameIndex].next;
            Waited_Ticks = 0;
            Animation_State = ArtMonster.Frames[FrameIndex].State;
        }

        public void TakeDamage(SkillObject skill, float Hit_X = -10,float Hit_Y = 300) {
            float Physical, Fire, Cold, Lightning, Chaos;
            float duration;

            if (Shock == true) {
                Physical = skill.Physical * 1.5f;
                Fire = skill.Fire * 1.5f;
                Cold = skill.Cold * 1.5f;
                Lightning = skill.Lightning * 1.5f;
                Chaos = skill.Chaos * 1.5f;
            } else {
                Physical = skill.Physical;
                Fire = skill.Fire;
                Cold = skill.Cold;
                Lightning = skill.Lightning;
                Chaos = skill.Chaos;
            }

            

            if (skill.Ability == AbilityType.Attack && (skill.Knockback_on_Crit == true || GameSetting.ProcChance(skill.Chance_Knockback) == true)) {
                Acceleration = 2 * skill.Knockback_Distance / Base_Knockback_Duration / Base_Knockback_Duration;
                Knockback_Duration = Base_Knockback_Duration;
                Knockback_Speed = Acceleration * Knockback_Duration;    
            }

            if (Physical != 0) {
                Physical = Physical * (1 - 0.06f * Stats.Armor / (1 + 0.06f * Stats.Armor));
                float Percent_Hit = Physical / this.Stats.Life_Maximum;
                if (Percent_Hit >= 0.1 * (1 - skill.Recalculation_Reduced_Enemy_Stun_Threshold) && GameSetting.ProcChance(2 * Percent_Hit) == true) {
                    duration = Base_Stun_Duration * (1 + skill.Increased_Stun_Duration);
                    if (Stun == false) { Stun = true; Stun_Duration = duration; SetMoveState(MoveState.Stun); }
                    else { if (duration > Stun_Duration) { Stun_Duration = duration; } }
                }
            }

            if (Fire != 0) { 
                if (skill.IsCrit == true || GameSetting.ProcChance(skill.Chance_Ignite) == true) {
                    Ignite_DPS = (Ignite_DPS * Ignite_Duration + Fire * 0.2f * (Base_Ignite_Duration * (1 + skill.Increased_Ignite_Duration) - Ignite_Duration)) / Base_Ignite_Duration / (1 + skill.Increased_Ignite_Duration);
                    Ignite_Duration = Base_Ignite_Duration * (1 + skill.Increased_Ignite_Duration);
                    Ignite = true;
                }
            }

            if (Cold != 0) {
                duration = Base_Chill_Duration * (1 + skill.Increased_Chill_Duration) * Cold / Stats.Life_Maximum;
                if (duration >= Capped_Duration && duration > Chill_Duration) { Chill_Duration = duration; Chill = true; }
                if (skill.IsCrit == true || GameSetting.ProcChance(skill.Chance_Freeze)) {
                    duration = Base_Freeze_Duration * (1 + skill.Increased_Freeze_Duration) * Cold / Stats.Life_Maximum;
                    if (duration >= Capped_Duration && duration > Freeze_Duration) { 
                        Freeze_Duration = duration;
                        if (Freeze == false) { SetMoveState(MoveState.FreezeStart); Freeze = true; }
                    }
                } 
            }
            
            if (Lightning != 0) {
                if (skill.IsCrit == true || GameSetting.ProcChance(skill.Chance_Shock)) {
                    duration = Base_Shock_Duration * (1 + skill.Increased_Shock_Duration) * Lightning / Stats.Life_Maximum;
                    if (duration >= Capped_Duration && duration > Shock_Duration) { Shock_Duration = duration; Shock = true; }
                }
            }

            float DealHP = Physical + Fire + Cold + Lightning + Chaos;
            Life -= DealHP;
            if (skill.IsCrit == true) { e.All.Add(new Notification(Math.Round(DealHP).ToString(), Position.X, Position.Y - 40, 2, Fonts.FontinBold50, Color.Red)); }
            else { e.All.Add(new Notification(Math.Round(DealHP).ToString(), Position.X, Position.Y - 40, 2, Fonts.FontinBold12, Color.White)); }
            

            if (skill.Ability == AbilityType.Attack) { e.Player.OnHitMonster(this, DealHP); }
            if (Life <= 0) { DoDeath(Hit_X, Hit_Y); OnDeath(); }
        }
        public void TakeDamageOverTime(float Physical, float Fire, float Cold, float Lightning, float Chaos) {
            Life -= Physical + Fire + Cold + Lightning;
            if (Life <= 0) {
                if (Animation_State == MoveState.Walking) { Vx = -Stats.Movement_Speed_Walking * 2 / GameSetting.SecondPerFrame; }
                else if (Animation_State == MoveState.Running) { Vx = -Stats.Movement_Speed_Running * 2 / GameSetting.SecondPerFrame; }
                SetMoveState(MoveState.Death2); State = MonsterState.Death;
                OnDeath();
            }
        }
    }

    public class Bandit : Monster { 

        public Bandit(float X, float Y, MonsterStats stats, float Life_Percent, float Mana_Percent, MoveState animation_State) {
            ArtMonster = Images.Monster.Bandit;
            Position.X = X; Position.Y = Y; Stats = stats;
            Life = Stats.Life_Maximum * Life_Percent / 100;
            Mana = Stats.Mana_Maximum * Mana_Percent / 100;
            SetMoveState(animation_State);
        }

        public override void DrawOnGround() {
            if (State == MonsterState.Death) return;
            for (int i = 0; i < Auras.Count; i++) {
                Auras.Items[i].Draw(this);
            }
        }
        int TMP_int, Wait_Ticks;

        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
            
            if (Chill == true && Freeze == false) {
                
                e.SpriteBatch.Draw(ArtMonster.Image, Position,
                    ArtMonster.GetRect(ArtMonster.Frames[FrameIndex].pic), Opacity * new Color(200, 200, 255, 255), 0, ArtMonster.Origin, 1, SpriteEffects.FlipHorizontally, 0);
            } else {

                e.SpriteBatch.Draw(ArtMonster.Image, Position,
                    ArtMonster.GetRect(ArtMonster.Frames[FrameIndex].pic), Color.White * Opacity, 0, ArtMonster.Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            if (Ignite == true) { DrawIgnite(); }
            if (Shock == true) { DrawLightning(); }
            
            e.SpriteBatch.End();
        }

        public void DrawIgnite() {
            if (State == MonsterState.Alive) {
                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(5, 5), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(-15, 15), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);


                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(15, 10), new Rectangle(16 * TMP_int, 0, 16, 128), Color.White * Opacity,
                    0, new Vector2(16, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(0, 40), new Rectangle(16 * TMP_int, 0, 32, 128), Color.White * Opacity,
                    0, new Vector2(16, 64), 0.5f, SpriteEffects.None, 0);
            } else if (State == MonsterState.Death) {
                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(0, -10), new Rectangle(30 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(15, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(10, -10), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(-20, -10), new Rectangle(20 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(10, 64), 0.5f, SpriteEffects.None, 0);
            }

            Wait_Ticks++;
            if (Wait_Ticks >= 8) { TMP_int++; Wait_Ticks = 0; }
        }

        int Lightning_Wait_Ticks, Lightning_Frame;
        float Lightning_Time;
        float Angle1, Angle2;

        public void DrawLightning() {
            Lightning_Time += GameSetting.SecondPerFrame;
            Lightning_Wait_Ticks++;
            if (Lightning_Wait_Ticks % 6 == 0) { Lightning_Frame++; }
            if (Lightning_Wait_Ticks % 30 == 0) { Angle1 = (float)GameSetting.RND.NextFloat(0, 3.1416f); Angle2 = (float)GameSetting.RND.NextFloat(0, 3.1416f); }


            e.SpriteBatch.Draw(Images.Monster.Image_Light_Gradiant, Position, Images.Monster.Rect_Light_Gradiant, Color.White * Opacity * (0.6f + 0.2f * (float)Math.Abs(Math.Sin(Lightning_Time))), 0, Images.Monster.Origin_Light_Gradiant, 1, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Lightning1, Position, Images.Monster.Rect_Lightning1, Color.White * Opacity * (0.3f + 0.3f * (float)Math.Abs(Math.Sin(Lightning_Time))), 0, Images.Monster.Origin_Lightning1, 0.5f, SpriteEffects.None, 0);

            e.SpriteBatch.Draw(Images.Monster.Image_Lightning_Ball, Position, new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, 0.5f * Lightning_Frame, Images.Monster.Origin_Lightning_Ball, 0.5f, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Lightning_Ball, new Vector2(Position.X - 10, Position.Y + 10), new Rectangle(128 * (Lightning_Frame + 1), 0, 128, 128), Color.White * Opacity, 0.5f * Lightning_Frame, Images.Monster.Origin_Lightning_Ball, 0.25f, SpriteEffects.None, 0);

            e.SpriteBatch.Draw(Images.Monster.Image_Particle, Position, new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, Angle1, Images.Monster.Origin_Lightning_Ball, 0.75f, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Particle, new Vector2(Position.X, Position.Y + 10), new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, Angle2, Images.Monster.Origin_Lightning_Ball, 0.75f, SpriteEffects.None, 0);
        }
    }
    public class Hunter : Monster { 

        public Hunter(float X, float Y, MonsterStats stats, float Life_Percent, float Mana_Percent, MoveState animation_State) {
            ArtMonster = Images.Monster.Hunter;
            Position.X = X; Position.Y = Y; Stats = stats;
            Life = Stats.Life_Maximum * Life_Percent / 100;
            Mana = Stats.Mana_Maximum * Mana_Percent / 100;
            SetMoveState(animation_State);
        }

        public override void DrawOnGround() {
            if (State == MonsterState.Death) return;
            for (int i = 0; i < Auras.Count; i++) {
                Auras.Items[i].Draw(this);
            }
        }
        int TMP_int, Wait_Ticks;

        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
            
            if (Chill == true && Freeze == false) {
                
                e.SpriteBatch.Draw(ArtMonster.Image, Position,
                    ArtMonster.GetRect(ArtMonster.Frames[FrameIndex].pic), Opacity * new Color(200, 200, 255, 255), 0, ArtMonster.Origin, 1, SpriteEffects.FlipHorizontally, 0);
            } else {

                e.SpriteBatch.Draw(ArtMonster.Image, Position,
                    ArtMonster.GetRect(ArtMonster.Frames[FrameIndex].pic), Color.White * Opacity, 0, ArtMonster.Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            if (Ignite == true) { DrawIgnite(); }
            if (Shock == true) { DrawLightning(); }
            
            e.SpriteBatch.End();
        }
        public void DrawIgnite() {
            if (State == MonsterState.Alive) {
                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(5, 5), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(-15, 15), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);


                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(15, 10), new Rectangle(16 * TMP_int, 0, 16, 128), Color.White * Opacity,
                    0, new Vector2(16, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(0, 40), new Rectangle(16 * TMP_int, 0, 32, 128), Color.White * Opacity,
                    0, new Vector2(16, 64), 0.5f, SpriteEffects.None, 0);
            } else if (State == MonsterState.Death) {
                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(0, -10), new Rectangle(30 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(15, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(10, -10), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(-20, -10), new Rectangle(20 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(10, 64), 0.5f, SpriteEffects.None, 0);
            }

            Wait_Ticks++;
            if (Wait_Ticks >= 8) { TMP_int++; Wait_Ticks = 0; }
        }

        int Lightning_Wait_Ticks, Lightning_Frame;
        float Lightning_Time;
        float Angle1, Angle2;

        public void DrawLightning() {
            Lightning_Time += GameSetting.SecondPerFrame;
            Lightning_Wait_Ticks++;
            if (Lightning_Wait_Ticks % 6 == 0) { Lightning_Frame++; }
            if (Lightning_Wait_Ticks % 30 == 0) { Angle1 = (float)GameSetting.RND.NextFloat(0, 3.1416f); Angle2 = (float)GameSetting.RND.NextFloat(0, 3.1416f); }


            e.SpriteBatch.Draw(Images.Monster.Image_Light_Gradiant, Position, Images.Monster.Rect_Light_Gradiant, Color.White * Opacity * (0.6f + 0.2f * (float)Math.Abs(Math.Sin(Lightning_Time))), 0, Images.Monster.Origin_Light_Gradiant, 1, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Lightning1, Position, Images.Monster.Rect_Lightning1, Color.White * Opacity * (0.3f + 0.3f * (float)Math.Abs(Math.Sin(Lightning_Time))), 0, Images.Monster.Origin_Lightning1, 0.5f, SpriteEffects.None, 0);

            e.SpriteBatch.Draw(Images.Monster.Image_Lightning_Ball, Position, new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, 0.5f * Lightning_Frame, Images.Monster.Origin_Lightning_Ball, 0.5f, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Lightning_Ball, new Vector2(Position.X - 10, Position.Y + 10), new Rectangle(128 * (Lightning_Frame + 1), 0, 128, 128), Color.White * Opacity, 0.5f * Lightning_Frame, Images.Monster.Origin_Lightning_Ball, 0.25f, SpriteEffects.None, 0);

            e.SpriteBatch.Draw(Images.Monster.Image_Particle, Position, new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, Angle1, Images.Monster.Origin_Lightning_Ball, 0.75f, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Particle, new Vector2(Position.X, Position.Y + 10), new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, Angle2, Images.Monster.Origin_Lightning_Ball, 0.75f, SpriteEffects.None, 0);
        }
        public override void DoAttack() {
            SetMoveState(MoveState.Attacking1);
        }
        public override void OnAttack() {
            e.All.AddSort(new Monster_Normal_Arrow(this));
        }
    }

    public class Firen : Monster { 
        public Firen(float X, float Y, MonsterStats stats, float Life_Percent, float Mana_Percent, MoveState animation_State) {
            ArtMonster = Images.Monster.Firen;
            Position.X = X; Position.Y = Y; Stats = stats;
            Life = Stats.Life_Maximum * Life_Percent / 100;
            Mana = Stats.Mana_Maximum * Mana_Percent / 100;
            SetMoveState(animation_State);
        }

        public override void DrawOnGround() {
            if (State == MonsterState.Death) return;
            for (int i = 0; i < Auras.Count; i++) {
                Auras.Items[i].Draw(this);
            }
        }
        int TMP_int, Wait_Ticks;

        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
            
            if (Chill == true && Freeze == false) {
                
                e.SpriteBatch.Draw(ArtMonster.Image, Position,
                    ArtMonster.GetRect(ArtMonster.Frames[FrameIndex].pic), Opacity * new Color(200, 200, 255, 255), 0, ArtMonster.Origin, 1, SpriteEffects.FlipHorizontally, 0);
            } else {

                e.SpriteBatch.Draw(ArtMonster.Image, Position,
                    ArtMonster.GetRect(ArtMonster.Frames[FrameIndex].pic), Color.White * Opacity, 0, ArtMonster.Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            if (Ignite == true) { DrawIgnite(); }
            if (Shock == true) { DrawLightning(); }
            
            e.SpriteBatch.End();
        }

        public void DrawIgnite() {
            if (State == MonsterState.Alive) {
                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(5, 5), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(-15, 15), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);


                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(15, 10), new Rectangle(16 * TMP_int, 0, 16, 128), Color.White * Opacity,
                    0, new Vector2(16, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(0, 40), new Rectangle(16 * TMP_int, 0, 32, 128), Color.White * Opacity,
                    0, new Vector2(16, 64), 0.5f, SpriteEffects.None, 0);
            } else if (State == MonsterState.Death) {
                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(0, -10), new Rectangle(30 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(15, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(10, -10), new Rectangle(10 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(5, 64), 0.5f, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Monster.Image_Fire2, Position - new Vector2(-20, -10), new Rectangle(20 * TMP_int, 0, 10, 128), Color.White * Opacity,
                    0, new Vector2(10, 64), 0.5f, SpriteEffects.None, 0);
            }

            Wait_Ticks++;
            if (Wait_Ticks >= 8) { TMP_int++; Wait_Ticks = 0; }
        }

        int Lightning_Wait_Ticks, Lightning_Frame;
        float Lightning_Time;
        float Angle1, Angle2;

        public void DrawLightning() {
            Lightning_Time += GameSetting.SecondPerFrame;
            Lightning_Wait_Ticks++;
            if (Lightning_Wait_Ticks % 6 == 0) { Lightning_Frame++; }
            if (Lightning_Wait_Ticks % 30 == 0) { Angle1 = (float)GameSetting.RND.NextFloat(0, 3.1416f); Angle2 = (float)GameSetting.RND.NextFloat(0, 3.1416f); }


            e.SpriteBatch.Draw(Images.Monster.Image_Light_Gradiant, Position, Images.Monster.Rect_Light_Gradiant, Color.White * Opacity * (0.6f + 0.2f * (float)Math.Abs(Math.Sin(Lightning_Time))), 0, Images.Monster.Origin_Light_Gradiant, 1, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Lightning1, Position, Images.Monster.Rect_Lightning1, Color.White * Opacity * (0.3f + 0.3f * (float)Math.Abs(Math.Sin(Lightning_Time))), 0, Images.Monster.Origin_Lightning1, 0.5f, SpriteEffects.None, 0);

            e.SpriteBatch.Draw(Images.Monster.Image_Lightning_Ball, Position, new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, 0.5f * Lightning_Frame, Images.Monster.Origin_Lightning_Ball, 0.5f, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Lightning_Ball, new Vector2(Position.X - 10, Position.Y + 10), new Rectangle(128 * (Lightning_Frame + 1), 0, 128, 128), Color.White * Opacity, 0.5f * Lightning_Frame, Images.Monster.Origin_Lightning_Ball, 0.25f, SpriteEffects.None, 0);

            e.SpriteBatch.Draw(Images.Monster.Image_Particle, Position, new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, Angle1, Images.Monster.Origin_Lightning_Ball, 0.75f, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Monster.Image_Particle, new Vector2(Position.X, Position.Y + 10), new Rectangle(128 * Lightning_Frame, 0, 128, 128), Color.White * Opacity, Angle2, Images.Monster.Origin_Lightning_Ball, 0.75f, SpriteEffects.None, 0);
        }
        public override void OnNextFrame() {
            if (400 < Position.X && Position.X <= 1000 && Mana >= 75 && (Animation_State == MoveState.Walking || Animation_State == MoveState.Running)) {
                SetMoveState(MoveState.Cast);
                
            } else if (ArtMonster.Frames[FrameIndex].Can_Jump == true && Mana >= 75) {
                FrameIndex = ArtMonster.Frames[FrameIndex].Next_Jump;
                Animation_State = ArtMonster.Frames[FrameIndex].State;
                Waited_Ticks = 0;
            } else if (Position.X <= 210 && Mana >= 75 && Animation_State == MoveState.Standing) {
                SetMoveState(MoveState.Custom1);
            }
            else
            {
                FrameIndex = ArtMonster.Frames[FrameIndex].next;
                Animation_State = ArtMonster.Frames[FrameIndex].State;
                Waited_Ticks = 0;
            }

            if (Animation_State == MoveState.Cast && ArtMonster.Frames[FrameIndex].IsCast == true) {
                Mana -= 75; e.All.AddSort(new Monster_Fireball(this));
            } else if (Animation_State == MoveState.Custom1 && ArtMonster.Frames[FrameIndex].IsCast == true) {
                Mana -= 75; e.All.AddSort(new Monster_Explosive(this));
                TakeDamageOverTime(0, Life * 0.3f, 0, 0, 0);
            }

        }
    }

    public class Monster_Fireball : GameObject {
        public static int Tick_Per_Frame = 4;
        public Monster Monster;
        public Monster_Fireball(Monster monster) { 
            Monster = monster;
            Position.X = monster.Position.X - 10;
            Position.Y = monster.Position.Y + 1;
        }

        
        public int Frame_Index;
        public int Frame_Wait;
        private MyState State;
        private enum MyState { Flying, Hit }

        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);
            e.SpriteBatch.Draw(Images.Monster.Skill.Fireball.Image, Position, Images.Monster.Skill.Fireball.Rects[Frame_Index], Color.White, 0, Images.Monster.Skill.Fireball.Origin, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }
        public override void Update() {
            if (State == MyState.Flying) {
                Position.X -= 10;
                Frame_Wait++;
                if (Frame_Wait >= Tick_Per_Frame) {
                    Frame_Wait = 0;
                    Frame_Index++; if (Frame_Index == 6) { Frame_Index = 0; }
                }
                if (Position.X <= 180) { State = MyState.Hit; HitPlayer(); Frame_Index = 6; }
            } else if (State == MyState.Hit) {
                Frame_Wait++;
                if (Frame_Wait >= Tick_Per_Frame) {
                    Frame_Wait = 0;
                    Frame_Index++; if (Frame_Index == 10) { Deletable = true; }
                }
            }
        }
        public void HitPlayer() {
            e.Player.TakeDamage(Monster,
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Physical_Damage[0], Monster.Stats.Attack_Physical_Damage[1]),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Fire_Damage[0], Monster.Stats.Attack_Fire_Damage[1]),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Cold_Damage[0], Monster.Stats.Attack_Cold_Damage[1]),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Lightning_Damage[0], Monster.Stats.Attack_Lightning_Damage[1]),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Chaos_Damage[0], Monster.Stats.Attack_Chaos_Damage[1]),
                Monster.Stats.Critical_Strike_Chance, Monster.Stats.Critical_Strike_Multiplier);
        }
    }
    public class Monster_Explosive : GameObject {
        public static int Tick_Per_Frame = 4;
        public Monster Monster;
        public Monster_Explosive(Monster monster) { 
            Monster = monster;
            Position.X = monster.Position.X;
            Position.Y = monster.Position.Y + 20;
        }

        
        public int Frame_Index;
        public int Frame_Wait;

        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);
            e.SpriteBatch.Draw(Images.Monster.Skill.Explosive.Image, Position, Images.Monster.Skill.Explosive.Rects[Frame_Index], Color.White, 0, Images.Monster.Skill.Explosive.Origin, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }
        public override void Update() {
            Frame_Wait++;
            if (Frame_Wait >= Tick_Per_Frame) {
                Frame_Wait = 0;
                Frame_Index++; if (Frame_Index == 11) { Deletable = true; }
                HitPlayer();
            }
        }
        public void HitPlayer() {
            e.Player.TakeDamage(Monster,
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Physical_Damage[0] * 0.2f, Monster.Stats.Attack_Physical_Damage[1] * 0.2f),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Fire_Damage[0] * 0.2f, Monster.Stats.Attack_Fire_Damage[1] * 0.2f),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Cold_Damage[0] * 0.2f, Monster.Stats.Attack_Cold_Damage[1] * 0.2f),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Lightning_Damage[0] * 0.2f, Monster.Stats.Attack_Lightning_Damage[1] * 0.2f),
                GameSetting.RND.NextFloat(Monster.Stats.Attack_Chaos_Damage[0] * 0.2f, Monster.Stats.Attack_Chaos_Damage[1] * 0.2f),
                Monster.Stats.Critical_Strike_Chance, Monster.Stats.Critical_Strike_Multiplier);
        }
    }
    public class Monster_Normal_Arrow : GameObject {
        public static float Base_Speed = 15;
        public Monster Monster;
        public float Stop_X;
        public Monster_Normal_Arrow(Monster monster) {
            Monster = monster;
            Position.X = Monster.Position.X - 30;
            Position.Y = Monster.Position.Y;
            Stop_X = GameSetting.RND.NextFloat(150, 180);
        }
        MyState State = MyState.Flying;
        private enum MyState { Flying, Hit }
        public float Opacity = 1;
        public override void Update() {
            if (State == MyState.Flying) { 
                Position.X -= Base_Speed;

                if (Position.X <= Stop_X) { State = MyState.Hit; HitPlayer(); }
            
            } else if (State == MyState.Hit) {
                Opacity -= 0.001f;
                if (Opacity <= 0) { Deletable = true; }
            }
        }
        public void HitPlayer() {
            e.Player.TakeDamage(Monster,
            GameSetting.RND.NextFloat(Monster.Stats.Attack_Physical_Damage[0], Monster.Stats.Attack_Physical_Damage[1]),
            GameSetting.RND.NextFloat(Monster.Stats.Attack_Fire_Damage[0], Monster.Stats.Attack_Fire_Damage[1]),
            GameSetting.RND.NextFloat(Monster.Stats.Attack_Cold_Damage[0], Monster.Stats.Attack_Cold_Damage[1]),
            GameSetting.RND.NextFloat(Monster.Stats.Attack_Lightning_Damage[0], Monster.Stats.Attack_Lightning_Damage[1]),
            GameSetting.RND.NextFloat(Monster.Stats.Attack_Chaos_Damage[0], Monster.Stats.Attack_Chaos_Damage[1]),
            Monster.Stats.Critical_Strike_Chance, Monster.Stats.Critical_Strike_Multiplier);
        }

        public override void DrawOnTop() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);
            if (State == MyState.Flying) {
                e.SpriteBatch.Draw(Skills.Normal_Arrow.Image_Trail, new Vector2(Position.X + 40, Position.Y),
                        Skills.Normal_Arrow.Rect_Trail, Color.White, 0, Skills.Normal_Arrow.Origin_Trail, new Vector2(1, 0.1f), SpriteEffects.None, 0);
            } else if (State == MyState.Hit) {
                
            }

            e.SpriteBatch.Draw(Skills.Normal_Arrow.Image_Arrow, Position, Skills.Normal_Arrow.Rect_Arrow, Color.White * Opacity, 0, Skills.Normal_Arrow.Origin_Arrow, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }
    }
    public class Notification : GameObject {
        public string Text;
        public float FadeTime, Time;
        public SpriteFont Font;
        public Color Color;

        public Notification(string text, float x, float y, float fadeTime, SpriteFont font, Color color) {
            Text = text;
            FadeTime = fadeTime;
            Position.X = x;
            Position.Y = y;
            Font = font;
            Color = color;
        }
        public override void Update() {
            Position.Y -= 1;
            Time += GameSetting.SecondPerFrame;
            if (Time >= FadeTime) { Deletable = true; }
        }
        public override void DrawOnTop2() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            e.SpriteBatch.DrawStringCenter(Font, Text, Position, Color * ((FadeTime - Time) / FadeTime), 0, 1, SpriteEffects.None);
            
            e.SpriteBatch.End();
        }
    }

    public abstract class Aura {
        public float Radius;
        public float Value;
        /// <summary>Aura.Draw sẻ gọi SpriteBatch.Begin và SpriteBatch.End</summary>
        public abstract void Draw(Monster monster);
        public virtual void Update() { 
            
        }
    }
    public class AuraCollection {
        public Aura[] Items = { };

        public int Count { get { return Items.Length; } }
        public void Add(Aura item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }

        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Clear() { Array.Resize(ref  Items, 0); }
        public int IndexOf(Aura aura) {
            return Array.IndexOf(Items, aura);
        }

    }
    public class EnergySheild : Aura {
        public static RenderTarget2D TMP_Texture2D;
        public static Vector2 Scale = new Vector2(1, 3f / 8);

        public EnergySheild(){}
        static EnergySheild() {
            TMP_Texture2D = RenderTarget2D.New(e.GraphicsDevice, 128, 128, PixelFormat.B8G8R8A8.UNorm);
        }
        SharpDX.Direct3D11.DepthStencilView depthStencilView;
        SharpDX.Direct3D11.RenderTargetView[] renderTargets;

        float Rotation;
        public override void Update() {
            Rotation += 0.1f;
        }

        public override void Draw(Monster monster) {
            renderTargets = e.GraphicsDevice.GetRenderTargets(out depthStencilView);
            e.GraphicsDevice.SetRenderTargets(TMP_Texture2D);
            e.GraphicsDevice.Clear(Color.Black);
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);
            e.SpriteBatch.Draw(Images.Skill.EnergySheild.Image, Images.Skill.EnergySheild.Origin, Images.Skill.EnergySheild.Rect,
                Color.White, Rotation, Images.Skill.EnergySheild.Origin, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
            e.GraphicsDevice.SetRenderTargets(depthStencilView, renderTargets);

            //Aura
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);
            e.SpriteBatch.Draw(TMP_Texture2D, monster.Position, Images.Skill.EnergySheild.Rect, Color.White, 0, Images.Skill.EnergySheild.Origin, Scale, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }

    }
}
