(function ($, undefined) {
    function SvgTextWidthSimulator(textElement) {
        this._textElement = textElement;
        this._jqTextElement = $(textElement);
        this._svgNs = "http://www.w3.org/2000/svg";
    }

    SvgTextWidthSimulator.prototype._loadWords = function () {
        var text = this._jqTextElement.text().trim();

        this._words = text.split(/\W+/);
    };

    SvgTextWidthSimulator.prototype._loadPositionInformation = function () {
        var bbox;
        var tspanIfExists = this._jqTextElement.find("tspan");
        if (tspanIfExists.length > 0) {
            bbox = tspanIfExists[0].getBBox();
        } else {
            bbox = this._textElement.getBBox();
        }

        this._x = bbox.x;
        this._lineHeight = "1.2em";
    };

    SvgTextWidthSimulator.prototype.setWidth = function (width) {
        this._loadWords();
        this._loadPositionInformation();
        this._jqTextElement.empty();

        while (this._words.length > 0) {
            this._addLine(width);
        }
    };

    SvgTextWidthSimulator.prototype._addLine = function (width) {
        var tspan = this._createTspan();
        this._jqTextElement.append(tspan);

        var previousLength = 0;
        while (this._words.length > 0) {
            var candidateWord = this._words.shift();
            var previousCandidate = tspan.textContent;

            tspan.textContent = previousCandidate + " " + candidateWord;
            var currentLength = tspan.getComputedTextLength();
            if (currentLength > width) {
                if (previousLength !== currentLength) {
                    tspan.textContent = previousCandidate;
                    this._words.unshift(candidateWord);
                }

                break;
            } else {
                previousLength = currentLength;
            }
        }

        return tspan;
    };

    SvgTextWidthSimulator.prototype._createTspan = function () {
        var tspan = document.createElementNS(this._svgNs, "tspan");
        tspan.setAttributeNS(null, "x", this._x);

        if (this._jqTextElement.find("tspan").length > 0) {
            tspan.setAttributeNS(null, "dy", this._lineHeight);
        }

        return tspan;
    };

    window.SvgTextWidthSimulator = SvgTextWidthSimulator;
})(jQuery);