using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimhashLib
{
    public class SimhashIndex
    {
        private const int FpSizeStatic = 64;
        
        private readonly int _kDistance;
        private readonly int _fpSize;
        private readonly Dictionary<string, HashSet<string>> _bucket;
        private static List<int> _offsets;

        //whitepaper says 64 and 3 are optimal. the ash tray says you've been up all night...
        public SimhashIndex(Dictionary<long, SimhashResult> objs, int f = 64, int k = 3)
        {
            _kDistance = k;
            _fpSize = f;
            
            _bucket = new Dictionary<string, HashSet<string>>(objs.Count);

            _offsets = MakeOffsets();

            foreach (var q in objs)
                Add(q.Key, q.Value);
        }

        public HashSet<long> GetNearDups(SimhashResult simhash)
        {
            /*
            "simhash" is an instance of Simhash
            return a list of obj_id, which is in type of long (for now)
            */
            if (Simhash.FpSize != _fpSize) 
                throw new Exception();
            
            var ans = new HashSet<long>();

            foreach (var key in GetEnumerableKeys(simhash))
            {
                if (!_bucket.TryGetValue(key, out var dups)) 
                    continue;
                
                foreach (var dup in dups)
                {
                    var parts = dup.Split(',');
                    
                    var fp = Convert.ToUInt64(parts[0]);
                    var objId = Convert.ToInt64(parts[1]);
                    
                    var sim2 = new SimhashResult(fp);
                    
                    var d = simhash.Distance(sim2);
                    if (d <= _kDistance)
                    {
                        ans.Add(objId);
                    }
                }
            }
            return ans;
        }
        
        public void Add(long objId, SimhashResult simhash)
        {
            var store = new StringBuilder(41);
            foreach (var key in GetEnumerableKeys(simhash))
            {
                var v = BuildKey(objId, simhash, store);
                if (!_bucket.TryGetValue(key, out var values))
                {
                    var bucketHashSet = new HashSet<string>() { v };
                    _bucket.Add(key, bucketHashSet);
                }
                else
                {
                    values.Add(v);
                }
            }
        }
        
        public void Delete(long objId, SimhashResult simhash)
        {
            var store = new StringBuilder(41);
            foreach (var key in GetEnumerableKeys(simhash))
            {
                var v = BuildKey(objId, simhash, store);
                
                if (_bucket.TryGetValue(key, out var set))
                    set.Remove(v);
            }
        }

        public List<int> MakeOffsets()
        {
            /*
            You may optimize this method according to < http://www.wwwconference.org/www2007/papers/paper215.pdf>
            */
            //int optimizedSize = 4; replace kDistance with this var.
            //
            var ans = new List<int>();
            for (var i = 0; i < (_kDistance + 1); i++)
            {
                var offset = _fpSize / (_kDistance + 1) * i;
                ans.Add(offset);
            }
            return ans;
        }

        public List<string> GetListKeys(SimhashResult simhash) => GetEnumerableKeys(simhash).ToList();

        private static IEnumerable<string> GetEnumerableKeys(SimhashResult simhash)
        {
            if(_offsets.Count <= 0)
                yield break;

            var store = new StringBuilder(41);
            
            for (var i = 0; i < _offsets.Count; i++)
            {
                var off = i == (_offsets.Count - 1) ? FpSizeStatic - _offsets[i] : _offsets[i + 1] - _offsets[i];

                var m = (Math.Pow(2, off)) - 1;
                var m64 = Convert.ToUInt64(m);
                
                var c = simhash.Value >> _offsets[i] & m64;
                
                yield return store.Clear().Append(c).Append(',').Append(i).ToString();
            }
        }
        
        private static string BuildKey(long objId, SimhashResult simhash, StringBuilder builder) 
            => builder.Clear().Append(simhash.Value).Append(',').Append(objId).ToString();
    }
}
