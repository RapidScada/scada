// Check that the browser is up to date
function checkBrowser() {
    // check JavaScript support
    try {
        // supports for...of
        eval("var arr = []; for (var x of arr) {}");
        // supports Map object
        eval("var map = new Map(); map.set(1, 1); map.get(1); ");
        return true;
    }
    catch (ex) {
        return false;
    }
}