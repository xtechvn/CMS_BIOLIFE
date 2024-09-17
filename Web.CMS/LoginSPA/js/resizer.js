// JavaScript source code
/**
 * The resizer is Javascript object for resizing of the HTML element by mouse dragging.
 * Author: Andrej Hristoliubov
 * email: anhr@mail.ru
 * About me: https://anhr.github.io/AboutMe/
 * Sources: https://github.com/anhr/resizer
 *          http://jsfiddle.net/3jMQD/ 
 *          http://stackoverflow.com/questions/8960193/how-to-make-html-element-resizable-using-pure-javascript
 * Thanks to: http://stackoverflow.com/users/27862/user123444555621
 * Licences: GPL, The MIT License (MIT)
 * Copyright: (c) 2015 Andrej Hristoliubov
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * Revision:
 *  2015-11-1, : 
 *       + Start
 *
 */
function MessageElement(Message) {
    var element = document.getElementById('Message');
    if (element == null) {
        alert('ERROR: element Message == null. ' + Message);
        return;
    }
    if (element.innerHTML != Message) {
        //consoleLog('Message: ' + Message);
        element.innerHTML = Message + '<hr><div align="center"><input type="button" onclick="javascript: return MessageElement(\'\')" value="Close" style="margin-top:5px;" /></div>';
        if (Message == "")
            element.style.display = "none";
        else element.style.display = "block";
    }
}

function ErrorMessage(message, emailMe) {
    consoleError(message);

    //http://www.htmlhelp.com/reference/html40/entities/special.html
    var body;
    if (emailMe != false) {
        body = message;
        body = body.replace(/\n/g, "%0D%0A");
        body = body.replace(/ /g, "%20");
        body = body.replace(/"/g, "%22");
        body = body.replace(/&/g, "%26");
    }
    message = message.replace(/</g, '&lt;');
    message = message.replace(/>/g, '&gt;');

    message = message.replace(/\n/g, '<BR>');
    if (emailMe != false) {
        //http://www.rapidtables.com/web/html/mailto.htm
        if (typeof myEmail == 'undefined')
            myEmail = "anhr@mail.ru";
        message += "<BR><BR><a href=\"mailto:" + myEmail + "?subject=" + emailSubject + "&body=" + body + "\" >Email me about problem</a>"
    }
    MessageElement('<FONT style="color: red; background-color: white">ERROR: ' + message + '</FONT>');
}

function consoleLog(message) {
    // try {
    //     console.log(message); 
    // } catch (e) {}
}

function consoleError(msg) {
    try {
        console.error(msg);
    } catch (e) {
        //		alert(msg);
    }
}


function resizer(resizerID, mousemove, cursor) {
    consoleLog("resizer(" + resizerID + ")");
    var resizer = document.getElementById(resizerID);
    resizer.style.cursor = cursor;
    resizer.mousemove = mousemove;

    resizer.onmousedown = function (e) {
        try {
            consoleLog("resizer.onmousedown(e)");
            document.documentElement.addEventListener('mousemove', resizer.doDrag, false);
            document.documentElement.addEventListener('mouseup', resizer.stopDrag, false);
        } catch (e) {
            ErrorMessage("resizer.onmousedown(...) failed! Your browser does not support this feature. " + e.message);
        }
    }

    resizer.doDrag = function (e) {
        if (e.which != 1) {
            consoleLog("mouseup");
            resizer.stopDrag(e);
            return;
        }
        //consoleLog("doDrag(e)");
        resizer.mousemove(e);
    }

    resizer.stopDrag = function (e) {
        consoleLog("stopDrag(e)");
        document.documentElement.removeEventListener('mousemove', resizer.doDrag, false);
        document.documentElement.removeEventListener('mouseup', resizer.stopDrag, false);
    }
}

function resizerX(resizerID, mousemove) {
    resizer(resizerID, mousemove, "e-resize");
}

function resizerY(resizerID, mousemove) {
    resizer(resizerID, mousemove, "n-resize");
}

function resizerXY(resizerID, mousemove) {
    resizer(resizerID, mousemove, "ne-resize");
}