using System.Collections.Generic;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class HoraxGasMapComponenet : MapComponent
    {
        private HashSet<IntVec3> horaxGasCells = new HashSet<IntVec3>();
        IntVec3[] cardinalDirections;

        public HoraxGasMapComponenet(Map map) : base(map)
        {
            cardinalDirections = GenAdj.CardinalDirections;
        }

        public bool HasAny
        {
            get 
            { 
                return !horaxGasCells.NullOrEmpty(); 
            }
        }

        public void Add(IntVec3 cell)
        {
            if (HasHoraxGas(cell) == true)
            {
                return;
            }
            horaxGasCells.Add(cell);
        }

        public void Remove(IntVec3 cell)
        {
            if (HasHoraxGas(cell) == false)
            {
                return;
            }
            horaxGasCells.Remove(cell);
            if(HasAny == false)
            {
                GasGrid_SetDirect_Patch.Clear();
            }
        }

        public bool HasHoraxGas(IntVec3 cell)
        {
            return horaxGasCells.Contains(cell);
        }

        public void TryToSpread(IntVec3 cell)
        {
            if (HasHoraxGas(cell))
            {
                return;
            }

            for (int i = 0; i < cardinalDirections.Length; i++)
            {
                IntVec3 adjCell = cell + cardinalDirections[i];

                if (adjCell.InBounds(map) && HasHoraxGas(adjCell))
                {
                    Add(cell);
                    return;
                }
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref horaxGasCells, "horaxGasCells", LookMode.Value);

            if (Scribe.mode == LoadSaveMode.PostLoadInit && horaxGasCells == null)
            {
                horaxGasCells = new HashSet<IntVec3>();
            }
        }
    }

    public static class HoraxGasUtility
    {
        public static HoraxGasMapComponenet GetHoraxGas(this Map map)
        {
            return map.GetComponent<HoraxGasMapComponenet>();
        }
    }
}