using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct PositionResponse
    {
        public PositionResponse(Dictionary<string, dynamic> data)
        {
            Day = new List<Position>();
            Net = new List<Position>();

            foreach (Dictionary<string, dynamic> item in data["day"])
                Day.Add(new Position(item));
            foreach (Dictionary<string, dynamic> item in data["net"])
                Net.Add(new Position(item));
        }

        public List<Position> Day { get; }
        public List<Position> Net { get; }
    }
}
