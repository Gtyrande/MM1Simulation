using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootProbabilityTool
{
    public class MersenneTwister : Random
    {

        private const Int32 N = 624;

        private const Int32 M = 397;

        private const UInt32 MatrixA = 0x9908b0df; /* constant vector a */

        private const UInt32 UpperMask = 0x80000000; /* most significant w-r bits */

        private const UInt32 LowerMask = 0x7fffffff; /* least significant r bits */

        private const UInt32 TemperingMaskB = 0x9d2c5680;

        private const UInt32 TemperingMaskC = 0xefc60000;



        private readonly UInt32[] _mt = new UInt32[N]; /* the array for the state vector */

        private Int16 _mti;

        private static readonly UInt32[] _mag01 = { 0x0, MatrixA };

        //初始化seed
        public MersenneTwister(Int32 seed)
        {

            init((UInt32)seed);

        }

        //生成随机数
        public override Double NextDouble()
        {
            Double FiftyThreeBitsOf1s = 9007199254740991.0;

            Double Inverse53BitsOf1s = 1.0 / FiftyThreeBitsOf1s;

            Double OnePlus53BitsOf1s = FiftyThreeBitsOf1s + 1;

            Double InverseOnePlus53BitsOf1s = 1.0 / OnePlus53BitsOf1s;

            UInt64 a = (UInt64)GenerateUInt32() >> 5;

            UInt64 b = (UInt64)GenerateUInt32() >> 6;

            return ((a * 67108864.0 + b) + 0) * InverseOnePlus53BitsOf1s;
        }


        protected UInt32 GenerateUInt32()
        {

            UInt32 y;

            if (_mti >= N)
            {

                Int16 kk = 0;

                for (; kk < N - M; ++kk)
                {

                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);

                    _mt[kk] = _mt[kk + M] ^ (y >> 1) ^ _mag01[y & 0x1];

                }

                for (; kk < N - 1; ++kk)
                {

                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);

                    _mt[kk] = _mt[kk + (M - N)] ^ (y >> 1) ^ _mag01[y & 0x1];

                }

                y = (_mt[N - 1] & UpperMask) | (_mt[0] & LowerMask);

                _mt[N - 1] = _mt[M - 1] ^ (y >> 1) ^ _mag01[y & 0x1];

                _mti = 0;

            }

            y = _mt[_mti++];

            y ^= y >> 11;

            y ^= y << 7 & TemperingMaskB;

            y ^= y << 15 & TemperingMaskC;

            y ^= y >> 18;

            return y;

        }

        private void init(UInt32 seed)
        {

            _mt[0] = seed & 0xffffffffU;

            for (_mti = 1; _mti < N; _mti++)
            {

                _mt[_mti] = (uint)(1812433253U * (_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + _mti);

                _mt[_mti] &= 0xffffffffU;

            }

        }

    }

}

