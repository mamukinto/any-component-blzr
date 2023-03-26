using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace AnyComponent.Pages
{
    public class GameOfLifeViewModel : ComponentBase
    {

        protected int width  = 10;
        protected int height = 10;

        protected int NEIGHBOURS_TO_BIRTH = 3;
        protected int UNDERPOPULATION_LIMIT = 2;
        protected int OVERPOPULATION_LIMIT = 3;
        protected int DELAY_IN_MILLISECONDS = 250;

        public int Width { get => width; set { width = value; InitMap();  }  }
        public int Height { get => height; set { height = value; InitMap(); } }

        protected bool simulating = false;
        protected int cssWidth => 600 / width;
        protected int cssHeight => 600 / height;

        protected int[,] Map;

        protected override void OnParametersSet()
        {
            InitMap();   
            base.OnParametersSet();
        }

        private void InitMap() {
            Map = new int[width, height];
        }

        protected void ResetDefaults()
        {
            width = 10;
            height = 10;

            NEIGHBOURS_TO_BIRTH = 3;
            UNDERPOPULATION_LIMIT = 2;
            OVERPOPULATION_LIMIT = 3;
            DELAY_IN_MILLISECONDS = 250;
        }
        protected void OnCellClick(int x, int y)
        {
            Map[x, y] = Map[x, y] == 0 ? 1 : 0;

            StateHasChanged();
        }


        protected async void StartSimulation()
        {
            simulating = true;
            while (simulating) {
                Console.WriteLine("Step!");
                Step();
                StateHasChanged();
                await Task.Delay(DELAY_IN_MILLISECONDS);
            }
        }

        private void Step() {
            var neighborCountMap = new int[width,height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    neighborCountMap[i,j] = CountNeighbours(i,j);
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var isAlive = Map[i, j] == 1;
                    if (isAlive)
                    {
                        var count = neighborCountMap[i, j];
                        if (count < UNDERPOPULATION_LIMIT || count > OVERPOPULATION_LIMIT)
                        {
                            Map[i, j] = 0; //die
                        }
                        else {
                            Map[i, j] = 1; //survive
                        }
                    }
                    else
                    {
                        var count = neighborCountMap[i, j];
                        if (count == NEIGHBOURS_TO_BIRTH)
                        {
                            Map[i, j] = 1; //birth
                        } else
                        {
                            Map[i, j] = 0; //stay dead
                        }
                    }
                }
            }
            
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            Console.WriteLine("x:" + x + " y:" + y);
            if (x - 1 >= 0) // has left neighbour
            {
                if (Map[x - 1, y] == 1)
                {
                    Console.WriteLine("has left neighbour");
                    count++;
                }
            }

            if (x + 1 < width) // has right neighbour
            {
                if (Map[x + 1, y] == 1) { 
                    Console.WriteLine("has right neighbour");
                    count++;
                }
            }

            if (y - 1 >= 0) // has top neighbour
            {
                if (Map[x, y - 1] == 1)
                {
                    Console.WriteLine("has top neighbour");
                    count++;
                }
            }

            if (y + 1 < height) // has bottom neighbour
            {
                if (Map[x, y + 1] == 1)
                {
                    Console.WriteLine("has bottom neuighbour");
                    count++;
                }
            }

            if (y + 1 < height && x + 1 < width) // has bottom-right neighbour
            {
                if (Map[x + 1, y + 1] == 1)
                {
                    Console.WriteLine("has bottom-right neighbour");
                    count++;
                }
            }

            if (y - 1 >= 0 && x - 1 >= 0) // has top-left neighbour
            {
                if (Map[x - 1, y - 1] == 1)
                {
                    Console.WriteLine("has top-left neighbour");
                    count++;
                }
            }

            if (y + 1 < height && x - 1 >= 0) // has bottom-left neighbour
            {
                if (Map[x - 1, y + 1] == 1)
                {
                    Console.WriteLine("has bottom-left neighbour");
                    count++;
                }
            }

            if (y - 1 >= 0 && x + 1 < width) // has top-right neighbour
            {
                if (Map[x + 1, y - 1] == 1)
                {
                    Console.WriteLine("has top-right neighbour");
                    count++;
                }
            }

            return count;

        }
    }
}
