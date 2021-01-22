using Restpirators.Analyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using Restpirators.Common.Enums;

namespace Restpirators.Analyzer.DataAccess.EFCore
{
    public class EmergenciesRepository : IEmergenciesRepository 
    {
        private readonly IMongoDatabase _database;
        public EmergenciesRepository(IMongoDatabase db)
        {
            //var client = new MongoClient();
            _database = db;
        }
        public IEnumerable<DtoQuantityStatistic> GetEmergencyQuantityStatistics(int? year, int? month)
        {
            var col = _database.GetCollection<EmergencyHistory>("Statistics");
            List<BsonDocument> pipeline = new List<BsonDocument>();
            pipeline.Add(new BsonDocument("$match", new BsonDocument("Status", 4)));
            pipeline.Add(new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"_id", 0},
                                {"year", new BsonDocument("$year", "$ModDate")},
                                {"month", new BsonDocument("$month", "$ModDate")},
                                {"EmergencyTypeId", "$EmergencyTypeId" }
                            }
                    }
                });
            if (year.HasValue)
                pipeline.Add(new BsonDocument("$match", new BsonDocument("year", year)));
            if (month.HasValue)
                pipeline.Add(new BsonDocument("$match", new BsonDocument("month", month)));
            pipeline.Add(new BsonDocument
                {
                    { "$group",
                        new BsonDocument
                            {
                                { "_id", new BsonDocument
                                             {
                                                 { "Type","$EmergencyTypeId" },
                                             }
                                },
                                {
                                    "Count", new BsonDocument
                                                 {
                                                     { "$sum", 1 }
                                                 }
                                }
                        }
                }
                });
            pipeline.Add(new BsonDocument { { "$lookup", new BsonDocument { { "from", "EmergencyTypes" }, { "localField", "_id.Type" }, { "foreignField", "Id" }, { "as", "type" } } } });
            pipeline.Add(new BsonDocument { { "$unwind", "$type"} });
            pipeline.Add(new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Count", 1},
                                {"Name", "$type.Name"},
                                {"_id", 0}
                            }
                    }
                });
            return col.Aggregate<BsonDocument>(pipeline).ToList().Select(x => BsonSerializer.Deserialize<DtoQuantityStatistic>(x));
        }

        public IEnumerable<DtoTimeStatistic> GetEmergencyTimeStatistics(int? year, int? month)
        {
            try
            {
                var col = _database.GetCollection<EmergencyHistory>("Statistics");
                List<BsonDocument> criterias = new List<BsonDocument>();
                criterias.AddRange(new[]
                {
                new BsonDocument("$match", new BsonDocument("Status", new BsonDocument("$ne", 4))),
                new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"_id", 0},
                                {"year", new BsonDocument("$year", "$ModDate")},
                                {"month", new BsonDocument("$month", "$ModDate")},
                                {"EmergencyTypeId", 1 },
                                {"EmergencyId", 1 },
                                {"ModDate", 1 },
                                {"Status", 1 }
                            }
                    }
                },
            });
                if (year.HasValue)
                    criterias.Add(new BsonDocument("$match", new BsonDocument("year", year)));
                if (month.HasValue)
                    criterias.Add(new BsonDocument("$match", new BsonDocument("month", month)));
                criterias.AddRange(new[]
                {
                new BsonDocument { { "$lookup", new BsonDocument { { "from", "EmergencyTypes" }, { "localField", "EmergencyTypeId" }, { "foreignField", "Id" }, { "as", "type" } } } },
                new BsonDocument { { "$unwind", "$type" } },
                new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"EmergencyId", 1},
                                {"Status", 1},
                                {"ModDate", 1},
                                {"EmergencyType", "$type.Name"},
                                {"EmergencyTypeId", 1},
                                {"_id", 0}
                            }
                    }
                }
                });
                var result = col.Aggregate<BsonDocument>(criterias).ToList().Select(x => BsonSerializer.Deserialize<EmergencyTimeHistory>(x));
                //Dictionary<int, List<TimeSpan>> acceptedToNewStatusTimes = new Dictionary<int, List<TimeSpan>>();
                Dictionary<int, List<TimeSpan>> teamSentToNewStatusTimes = new Dictionary<int, List<TimeSpan>>();
                //Dictionary<int, List<TimeSpan>> teamSentToAcceptedStatusTimes = new Dictionary<int, List<TimeSpan>>();
                Dictionary<int, List<TimeSpan>> doneToNewStatusTimes = new Dictionary<int, List<TimeSpan>>();
                //Dictionary<int, List<TimeSpan>> doneToAcceptedStatusTimes = new Dictionary<int, List<TimeSpan>>();
                Dictionary<int, List<TimeSpan>> doneToTeamSentStatusTimes = new Dictionary<int, List<TimeSpan>>();
                foreach (var item in result.GroupBy(x => new { x.EmergencyId, x.EmergencyTypeId }))
                {
                    var newStatus = item.Where(x => x.Status == EmergencyStatus.New).FirstOrDefault();
                    //var acceptedStatus = item.Where(x => x.Status == EmergencyStatus.Accepted).FirstOrDefault();
                    var teamSentStatus = item.Where(x => x.Status == EmergencyStatus.TeamSent).FirstOrDefault();
                    var doneStatus = item.Where(x => x.Status == EmergencyStatus.Done).FirstOrDefault();
                    //if (newStatus != null && acceptedStatus != null)
                    //{
                    //    if (acceptedToNewStatusTimes.ContainsKey(item.Key.EmergencyTypeId))
                    //        acceptedToNewStatusTimes[item.Key.EmergencyTypeId].Add(acceptedStatus.ModDate - newStatus.ModDate);
                    //    else
                    //        acceptedToNewStatusTimes.Add(item.Key.EmergencyTypeId, new List<TimeSpan>() { acceptedStatus.ModDate - newStatus.ModDate });
                    //}
                    if (newStatus != null && teamSentStatus != null)
                    {
                        if (teamSentToNewStatusTimes.ContainsKey(item.Key.EmergencyTypeId))
                            teamSentToNewStatusTimes[item.Key.EmergencyTypeId].Add(teamSentStatus.ModDate - newStatus.ModDate);
                        else
                            teamSentToNewStatusTimes.Add(item.Key.EmergencyTypeId, new List<TimeSpan>() { teamSentStatus.ModDate - newStatus.ModDate });
                    }
                    //if (teamSentStatus != null && acceptedStatus != null)
                    //{
                    //    if (teamSentToAcceptedStatusTimes.ContainsKey(item.Key.EmergencyTypeId))
                    //        teamSentToAcceptedStatusTimes[item.Key.EmergencyTypeId].Add(teamSentStatus.ModDate - acceptedStatus.ModDate);
                    //    else
                    //        teamSentToAcceptedStatusTimes.Add(item.Key.EmergencyTypeId, new List<TimeSpan>() { teamSentStatus.ModDate - acceptedStatus.ModDate });
                    //}
                    if (doneStatus != null && newStatus != null)
                    {
                        if (doneToNewStatusTimes.ContainsKey(item.Key.EmergencyTypeId))
                            doneToNewStatusTimes[item.Key.EmergencyTypeId].Add(doneStatus.ModDate - newStatus.ModDate);
                        else
                            doneToNewStatusTimes.Add(item.Key.EmergencyTypeId, new List<TimeSpan>() { doneStatus.ModDate - newStatus.ModDate });
                    }
                    //if (doneStatus != null && acceptedStatus != null)
                    //{
                    //    if (doneToAcceptedStatusTimes.ContainsKey(item.Key.EmergencyTypeId))
                    //        doneToAcceptedStatusTimes[item.Key.EmergencyTypeId].Add(doneStatus.ModDate - acceptedStatus.ModDate);
                    //    else
                    //        doneToAcceptedStatusTimes.Add(item.Key.EmergencyTypeId, new List<TimeSpan>() { doneStatus.ModDate - acceptedStatus.ModDate });
                    //}
                    if (doneStatus != null && teamSentStatus != null)
                    {
                        if (doneToTeamSentStatusTimes.ContainsKey(item.Key.EmergencyTypeId))
                            doneToTeamSentStatusTimes[item.Key.EmergencyTypeId].Add(doneStatus.ModDate - teamSentStatus.ModDate);
                        else
                            doneToTeamSentStatusTimes.Add(item.Key.EmergencyTypeId, new List<TimeSpan>() { doneStatus.ModDate - teamSentStatus.ModDate });
                    }
                }
                var res = new List<DtoTimeStatistic>();
                //if (acceptedToNewStatusTimes.Count > 0)
                //{
                //    foreach (var item in acceptedToNewStatusTimes)
                //    {
                //        double doubleAverageTicks = acceptedToNewStatusTimes[item.Key].Average(timeSpan => timeSpan.Ticks);
                //        long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                //        res.Add(new DtoTimeStatistic()
                //        {
                //            StatisticType = Enums.TimeStatisticType.AcceptedToNew,
                //            TimeAverage = new TimeSpan(longAverageTicks),
                //            EmergencyType = result.FirstOrDefault(x => x.EmergencyTypeId == item.Key).EmergencyType
                //        });
                //    }
                //}
                if (teamSentToNewStatusTimes.Count > 0)
                {
                    foreach (var item in teamSentToNewStatusTimes)
                    {
                        double doubleAverageTicks = teamSentToNewStatusTimes[item.Key].Average(timeSpan => timeSpan.Ticks);
                        long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                        var t = TimeSpan.FromSeconds(new TimeSpan(longAverageTicks).TotalSeconds);
                        string timeAverage = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
                        res.Add(new DtoTimeStatistic()
                        {
                            StatisticType = Enums.TimeStatisticType.TeamSentToNew,
                            TimeAverage = timeAverage,
                            EmergencyType = result.FirstOrDefault(x => x.EmergencyTypeId == item.Key).EmergencyType
                        });
                    }
                }
                //if (teamSentToAcceptedStatusTimes.Count > 0)
                //{
                //    foreach (var item in teamSentToAcceptedStatusTimes)
                //    {
                //        double doubleAverageTicks = teamSentToAcceptedStatusTimes[item.Key].Average(timeSpan => timeSpan.Ticks);
                //        long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                //        res.Add(new DtoTimeStatistic()
                //        {
                //            StatisticType = Enums.TimeStatisticType.TeamSentToAccepted,
                //            TimeAverage = new TimeSpan(longAverageTicks),
                //            EmergencyType = result.FirstOrDefault(x => x.EmergencyTypeId == item.Key).EmergencyType
                //        });
                //    }
                //}
                if (doneToNewStatusTimes.Count > 0)
                {
                    foreach (var item in doneToNewStatusTimes)
                    {
                        double doubleAverageTicks = doneToNewStatusTimes[item.Key].Average(timeSpan => timeSpan.Ticks);
                        long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                        var t = TimeSpan.FromSeconds(new TimeSpan(longAverageTicks).TotalSeconds);
                        string timeAverage = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
                        res.Add(new DtoTimeStatistic()
                        {
                            StatisticType = Enums.TimeStatisticType.DoneToNew,
                            TimeAverage = timeAverage,
                            EmergencyType = result.FirstOrDefault(x => x.EmergencyTypeId == item.Key).EmergencyType
                        });
                    }
                }
                //if (doneToAcceptedStatusTimes.Count > 0)
                //{
                //    foreach (var item in doneToAcceptedStatusTimes)
                //    {
                //        double doubleAverageTicks = doneToAcceptedStatusTimes[item.Key].Average(timeSpan => timeSpan.Ticks);
                //        long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                //        res.Add(new DtoTimeStatistic()
                //        {
                //            StatisticType = Enums.TimeStatisticType.DoneToAccepted,
                //            TimeAverage = new TimeSpan(longAverageTicks),
                //            EmergencyType = result.FirstOrDefault(x => x.EmergencyTypeId == item.Key).EmergencyType
                //        });
                //    }
                //}
                if (doneToTeamSentStatusTimes.Count > 0)
                {
                    foreach (var item in doneToTeamSentStatusTimes)
                    {
                        double doubleAverageTicks = doneToTeamSentStatusTimes[item.Key].Average(timeSpan => timeSpan.Ticks);
                        long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                        var t = TimeSpan.FromSeconds(new TimeSpan(longAverageTicks).TotalSeconds);
                        string timeAverage = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
                        res.Add(new DtoTimeStatistic()
                        {
                            StatisticType = Enums.TimeStatisticType.DoneToTeamSent,
                            TimeAverage = timeAverage,
                            EmergencyType = result.FirstOrDefault(x => x.EmergencyTypeId == item.Key).EmergencyType
                        });
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return new[] { new DtoTimeStatistic() { EmergencyType = ex.Message } } ;
            }
        }

        public void InsertEmergencyHistory(EmergencyHistory emergencyHistory)
        {
            var col = _database.GetCollection<EmergencyHistory>("Statistics");
            col.InsertOne(emergencyHistory);
        }
    }
}
