using System.Collections.Generic;
using Match2.Core.Cube;
using Match2.Core.Grid;
namespace Match2.Core.Services.Matching
{
    public class CubeGrouper
    {
        public (List<BaseCube> normalCubes, List<BaseCube> obstacleCubes) GroupCubes(List<Cell> matches)
        {
            var normalCubes = new List<BaseCube>();
            var obstacleCubes = new List<BaseCube>();

            foreach (var cell in matches)
            {
                var cube = cell.CurrentCube;
                if (cube == null) continue;

                if (cube is ObstacleCube)
                    obstacleCubes.Add(cube);
                else
                    normalCubes.Add(cube);
            }

            return (normalCubes, obstacleCubes);
        }
    }
}