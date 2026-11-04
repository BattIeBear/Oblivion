using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace Oblivion_SlavesOfHorax
{
    public class LordJob_ConditionCauser_Ritual : LordJob
    {
        public Pawn ritualLeader;
        public IntVec3 ritualCell;
        public CompCauseGameCondition_Ritual ritualComp;

        private Sustainer sustainerOngoingHuman;
        private Sustainer sustainerOngoing;
        private Effecter effecterOngoing;

        private readonly List<Mote> motes = new List<Mote>();
        bool interrupted = false;
        public LordJob_ConditionCauser_Ritual()
        {
        }

        public LordJob_ConditionCauser_Ritual(Pawn ritualLeader, IntVec3 ritualCell, CompCauseGameCondition_Ritual ritualComp)
        {
            this.ritualLeader = ritualLeader;
            this.ritualCell = ritualCell;
            this.ritualComp = ritualComp;
        }

        public override StateGraph CreateGraph()
        {
            StateGraph graph = new StateGraph();

            LordToil_ConditionCauser_Ritual ritual = new LordToil_ConditionCauser_Ritual(ritualLeader, ritualCell);

            graph.AddToil(ritual);
            graph.StartingToil = ritual;

            return graph;
        }

        public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
        {
            base.Notify_PawnLost(p, condition);
            if (ritualLeader == p)
            {
                InterruptRitual();
            }
        }

        public override void Notify_PawnDowned(Pawn p)
        {
            base.Notify_PawnDowned(p);
            if (ritualLeader == p)
            {
                InterruptRitual();
            }
        }

        public override void LordJobTick()
        {
            base.LordJobTick();
            if (interrupted == true)
            {
                return;
            }
            TickEffects();
            TickSounds();
        }

        private void TickEffects()
        {
            if (effecterOngoing == null)
            {
                effecterOngoing = EffecterDefOf.PsychicRitual_Sustained.Spawn(ritualCell, ritualCell, Map);

                motes.Add(MoteMaker.MakeAttachedOverlay(ritualLeader, ThingDefOf.Mote_HateChantShadow, Vector3.zero));
                motes.Add(MoteMaker.MakeAttachedOverlay(ritualLeader, ThingDefOf.Mote_PsychicRitualInvocation, Vector3.zero));

                float radius = 4f;
                ThingDef[] candles =
                {
                    ThingDefOf.Mote_RitualCandleA,
                    ThingDefOf.Mote_RitualCandleB,
                    ThingDefOf.Mote_RitualCandleC
                };

                for (int i = 0; i < 6; i++)
                {
                    float angle = Mathf.PI * 2f * i / 6f;

                    IntVec3 cell = ritualCell;
                    cell.x += Mathf.RoundToInt(Mathf.Cos(angle) * radius);
                    cell.z += Mathf.RoundToInt(Mathf.Sin(angle) * radius);

                    motes.Add(MoteMaker.MakeStaticMote(cell, Map, Rand.Element(candles)));
                }
            }

            TargetInfo target = new TargetInfo(ritualCell, Map);

            effecterOngoing.EffectTick(target, target);

            foreach (Mote mote in motes)
            {
                mote?.Maintain();
            }

            if (Rand.MTBEventOccurs(8f, 60f, 1f))
            {
                SocialInteractionUtility.ImitateInteractionWithNoPawn(ritualLeader, InteractionDefOf.CreepyWords);
            }
        }

        private void TickSounds()
        {
            if (sustainerOngoingHuman == null || sustainerOngoingHuman.Ended)
            {
                sustainerOngoingHuman = SoundDefOf.PsychicRitual_Ongoing_Human.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(ritualCell, Map), MaintenanceType.PerTick));
            }

            if (sustainerOngoing == null || sustainerOngoing.Ended)
            {
                sustainerOngoing = SoundDefOf.PsychicRitual_Ongoing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(ritualCell, Map), MaintenanceType.PerTick));
            }

            sustainerOngoingHuman?.Maintain();
            sustainerOngoing?.Maintain();
        }

        private void InterruptRitual()
        {
            interrupted = true;
            ritualComp.parent.Destroy(DestroyMode.Vanish);

            SoundDefOf.PsychicRitual_Interrupted.PlayOneShot(SoundInfo.InMap(new TargetInfo(ritualCell, Map)));
            SoundDefOf.PsychicRitual_Interrupted_Human.PlayOneShot(SoundInfo.InMap(new TargetInfo(ritualCell, Map)));

            effecterOngoing?.Cleanup();
            effecterOngoing = null;

            if (sustainerOngoingHuman != null)
            {
                sustainerOngoingHuman.info.volumeFactor = 0f;
                sustainerOngoingHuman.End();
                sustainerOngoingHuman = null;
            }

            if (sustainerOngoing != null)
            {
                sustainerOngoing.info.volumeFactor = 0f;
                sustainerOngoing.End();
                sustainerOngoing = null;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_References.Look(ref ritualLeader, "ritualLeader");
            Scribe_Values.Look(ref ritualCell, "ritualCell");
            Scribe_Values.Look(ref ritualComp, "ritualComp");
        }
    }
}