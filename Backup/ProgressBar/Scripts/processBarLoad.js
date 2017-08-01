Type.registerNamespace('Jeffz.Sample');

Jeffz.Sample.LoadScripts = new function () {
    var totalCost = 0;
    var scriptLoader = new ScriptEngine();
    this.load = function (scripts) {
        if (Jeffz.Sample.__onScriptLoad != null) {
            throw new Error("In progress");
        }

        totalCost = 0;
        Jeffz.Sample.__onScriptLoad = onScriptLoad;
        var references = new Array();

        var loadedCost = 0;
        for (var i = 0; i < scripts.length; i++) {
            totalCost += scripts[i].cost;
            loadedCost += scripts[i].cost;

            var ref = createReference(scripts[i].url, loadedCost);

            references.push(ref);
        }
        scriptLoader.load(references, onComplete);
    }

    function createReference(url, loadedCost) {
        var ref = new Object();
        ref.url = url;
        ref.onscriptload = "Jeffz.Sample.__onScriptLoad('" + url + "', " + loadedCost + ")";
        return ref;
    }

    function onComplete() {
        Jeffz.Sample.__onScriptLoad = null;
    }

    function onScriptLoad(url, loadedCost) {
        var progress = 100.0 * loadedCost / totalCost;
        document.getElementById("bar").style.width = progress + "%";
        document.getElementById("message").innerHTML += ("<strong>" + url + "</strong>" + " loaded.<br />");
    }
}