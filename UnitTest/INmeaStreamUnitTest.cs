﻿/*
MIT License

Copyright (c) 2018 Inverness Park Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernessPark.Utilities.NMEA.UnitTest {
#if DEBUG
    [TestFixture]
    public class INmeaStreamUnitTest {
        private INmeaStream _strm;
        protected static readonly byte SOM = Convert.ToByte('$');

        private static readonly String[] _sampleData = new String[] {
            "$GPGGA,092750.000,5321.6802,N,00630.3372,W,1,8,1.03,61.7,M,55.2,M,,*76\r\n",
            "$GPGSA,A,3,10,07,05,02,29,04,08,13,,,,,1.72,1.03,1.38*0A\r\n",
            "$GPGSV,3,1,11,10,63,137,17,07,61,098,15,05,59,290,20,08,54,157,30*70\r\n",
            "$GPGSV,3,2,11,02,39,223,19,13,28,070,17,26,23,252,,04,14,186,14*79\r\n",
            "$GPGSV,3,3,11,29,09,301,24,16,09,020,,36,,,*76\r\n",
            "$GPRMC,092750.000,A,5321.6802,N,00630.3372,W,0.02,31.66,280511,,,A*43\r\n",
            "$GPGGA,092751.000,5321.6802,N,00630.3371,W,1,8,1.03,61.7,M,55.3,M,,*75\r\n",
            "$GPGSA,A,3,10,07,05,02,29,04,08,13,,,,,1.72,1.03,1.38*0A\r\n",
            "$GPGSV,3,1,11,10,63,137,17,07,61,098,15,05,59,290,20,08,54,157,30*70\r\n",
            "$GPGSV,3,2,11,02,39,223,16,13,28,070,17,26,23,252,,04,14,186,15*77\r\n",
            "$GPGSV,3,3,11,29,09,301,24,16,09,020,,36,,,*76\r\n",
            "$GPRMC,092751.000,A,5321.6802,N,00630.3371,W,0.06,31.66,280511,,,A*45\r\n",
        };

        public INmeaStreamUnitTest() {
            _strm = new NmeaStream(); // default implementation
        }

        protected INmeaStreamUnitTest(INmeaStream strm) {
            _strm = strm;
        }

        protected INmeaStream get_strm() {
            return _strm;
        }

        protected String[] get_sampleData() {
            return _sampleData;
        }

        [TestCase]
        public void ResetState() {
            _strm.Reset();
            Assert.AreEqual(_strm.Length, 0);
            Assert.AreEqual(_strm.Available, _strm.Capacity);
        }

        [TestCase]
        public void SaturatedState_NoSOM() {
            _strm.Reset();
            for (int i = 0; i < _strm.Capacity; ++i) {
                _strm.Append((byte)0x32);
            }
            Assert.AreEqual(_strm.Length, 0);
            Assert.AreEqual(_strm.Available, _strm.Capacity);
        }

        [TestCase]
        public void SaturatedState_WithSOM() {
            _strm.Reset();
            _strm.Append(SOM);
            for (int i = 0; i < _strm.Capacity - 1; ++i) {
                _strm.Append((byte)0x32);
            }
            Assert.AreEqual(_strm.Length, _strm.Capacity);
            Assert.AreEqual(_strm.Available, 0);
        }

        [TestCase]
        public void ContainsData() {
            foreach (String nmeaString in _sampleData) {
                _strm.Reset();
                // ... Don't finish the sentence
                byte[] nmeaBytes = Encoding.ASCII.GetBytes(nmeaString.Substring(0, nmeaString.Length - 2));
                Assert.AreEqual(nmeaString.Length - 2, nmeaBytes.Length, nmeaString);
                _strm.Append(nmeaBytes);
                Assert.AreEqual(_strm.Length, nmeaBytes.Length, nmeaString);
            }
        }
    }
#endif
}
