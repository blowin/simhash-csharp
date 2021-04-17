using System;
using System.Collections.Generic;
using System.Linq;

namespace SimhashLib
{
    public class SimhashIndex
    {
        private const int FpSizeStatic = 64;
        
        private readonly int _kDistance;
        private readonly int _fpSize = FpSizeStatic;
        private readonly Dictionary<string, HashSet<string>> _bucket;
        private static List<int> _offsets;

        //whitepaper says 64 and 3 are optimal. the ash tray says you've been up all night...
        public SimhashIndex(Dictionary<long, Simhash.Hash> objs, int f = 64, int k = 3)
        {
            _kDistance = k;
            _fpSize = f;
            
            _bucket = new Dictionary<string, HashSet<string>>(objs.Count);

            _offsets = MakeOffsets();

            foreach (var q in objs)
            {
                Add(q.Key, q.Value);
            }
        }

        public HashSet<long> GetNearDups(Simhash.Hash simhash)
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
                    var sim2 = new Simhash.Hash(fp);
                    var d = simhash.Distance(sim2);
                    if (d <= _kDistance)
                    {
                        ans.Add(objId);
                    }
                }
            }
            return ans;
        }
        
        public void Add(long objId, Simhash.Hash simhash)
        {
            foreach (var key in GetEnumerableKeys(simhash))
            {
                var v = string.Format("{0},{1}", simhash.Value, objId);
                if (!_bucket.ContainsKey(key))
                {
                    var bucketHashSet = new HashSet<string>() { v };
                    _bucket.Add(key, bucketHashSet);
                }
                else
                {
                    var values = _bucket[key];
                    values.Add(v);
                }
            }
        }

        public void Delete(long objId, Simhash.Hash simhash)
        {
            foreach (var key in GetEnumerableKeys(simhash))
            {
                var v = string.Format("{0},{1}", simhash.Value, objId);
                if (_bucket.ContainsKey(key))
                {
                    _bucket[key].Remove(v);
                }
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

        public List<string> GetListKeys(Simhash.Hash simhash)
        {
            return GetEnumerableKeys(simhash).ToList();
        }
        
        private static IEnumerable<string> GetEnumerableKeys(Simhash.Hash simhash)
        {
            for (var i = 0; i < _offsets.Count; i++)
            {
                int off;
                if (i == (_offsets.Count - 1))
                {
                    off = (FpSizeStatic - _offsets[i]);
                }
                else
                {
                    off = _offsets[i + 1] - _offsets[i];
                }

                var m = (Math.Pow(2, off)) - 1;
                var m64 = Convert.ToUInt64(m);
                var offset64 = Convert.ToUInt64(_offsets[i]);
                var c = simhash.Value >> _offsets[i] & m64;

                yield return string.Format("{0},{1}", c, i);
            }
        }
    }
}
