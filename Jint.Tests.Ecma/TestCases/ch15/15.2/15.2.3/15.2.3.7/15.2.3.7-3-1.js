/// Copyright (c) 2012 Ecma International.  All rights reserved. 
/**
 * @path ch15/15.2/15.2.3/15.2.3.7/15.2.3.7-3-1.js
 * @description Object.defineProperties - enumerable own data property of 'Properties' is defined in 'O' 
 */


function testcase() {

        var obj = {};      
        var props = {};
        Object.defineProperty(props, "prop", {
            value: {},
            enumerable: true
        });

        Object.defineProperties(obj, props);

        return obj.hasOwnProperty("prop");
    }
runTestCase(testcase);
