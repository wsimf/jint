// Copyright 2009 the Sputnik authors.  All rights reserved.
/**
 * The Date.prototype.setMinutes property "length" has { ReadOnly, DontDelete, DontEnum } attributes
 *
 * @path ch15/15.9/15.9.5/15.9.5.32/S15.9.5.32_A3_T1.js
 * @description Checking ReadOnly attribute
 */

x = Date.prototype.setMinutes.length;
Date.prototype.setMinutes.length = 1;
if (Date.prototype.setMinutes.length !== x) {
  $ERROR('#1: The Date.prototype.setMinutes.length has the attribute ReadOnly');
}


