/// Copyright (c) 2012 Ecma International.  All rights reserved. 
/**
 * @path ch15/15.4/15.4.4/15.4.4.22/15.4.4.22-3-22.js
 * @description Array.prototype.reduceRight throws TypeError exception when 'length' is an object with toString and valueOf methods that don�t return primitive values
 */


function testcase() {

        var accessed = false;
        var toStringAccessed = false;
        var valueOfAccessed = false;

        function callbackfn(prevVal, curVal, idx, obj) {
            accessed = true;
        }

        var obj = {
            0: 11,
            1: 12,

            length: {
                valueOf: function () {
                    valueOfAccessed = true;
                    return {};
                },
                toString: function () {
                    toStringAccessed = true;
                    return {};
                }
            }
        };

        try {
            Array.prototype.reduceRight.call(obj, callbackfn, 1);
            return false;
        } catch (ex) {
            return (ex instanceof TypeError) && toStringAccessed && valueOfAccessed && !accessed;
        }
    }
runTestCase(testcase);
