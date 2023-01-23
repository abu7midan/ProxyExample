
(function ($) {
	"use strict";

	if (!$) {
		throw "jQuery is required";
	}

	$.proxies = $.proxies || {
		baseUrl: "localhost:5254/"
	};

	function getQueryString(params, queryString) {
		queryString = queryString || "";
		for (var prop in params) {
			if (params.hasOwnProperty(prop)) {
				var val = getArgValue(params[prop]);
				if (val === null) continue;

				if ("" + val === "[object Object]") {
					queryString = getQueryString(params[prop], queryString);
					continue;
				}

				if (queryString.length) {
					queryString += "&";
				} else {
					queryString += "?";
				}
				queryString = queryString + prop + "=" + val;
			}
		}
		return queryString;
	}

	function getArgValue(val) {
		if (val === undefined || val === null) return null;
		return val;
	}

	function invoke(url, type, urlParams, body) {
		//url += getQueryString(urlParams);

		var ajaxOptions = $.extend({}, this.defaultOptions, {
			url: $.proxies.baseUrl + url,
			type: type,
			beforeSend: function (xhr) {
				if (typeof (webApiAuthToken) != "undefined" && webApiAuthToken.length > 0)
					xhr.setRequestHeader("Authorization", "Bearer " + webApiAuthToken);
			},
		});

		if (body) {
			ajaxOptions.data = body;
		}

		if (this.antiForgeryToken) {
			var token = $.isFunction(this.antiForgeryToken) ? this.antiForgeryToken() : this.antiForgeryToken;
			if (token) {
				ajaxOptions.headers = ajaxOptions.headers || {};
			}
		}

		return $.ajax(ajaxOptions);
	}

	function defaultAntiForgeryTokenAccessor() {
		return $("input[name=__RequestVerificationToken]").val();
	}

	function endsWith(str, suffix) {
		return str.indexOf(suffix, str.length - suffix.length) !== -1;
	}

	function appendPathDelimiter(url) {
		if (!endsWith(url, '/')) {
			return url + '/';
		}

		return url;
	}

	/* Proxies */

	$.proxies.weatherforecast = {
		defaultOptions: {},
		antiForgeryToken: defaultAntiForgeryTokenAccessor,



		get: function (options) {
			var defaults = { fields: [] };
			var settings = $.extend({}, defaults, options || {});
			var url = "WeatherForecast";

			if (settings.fields.length > 0) {
				url += url.indexOf("?") == -1 ? "?" : "&";
				url += "fields=" + settings.fields.join();
			}

			return invoke.call(this, url, "get",
				{}
			);
		},



		post: function (wether, options) {
			var defaults = { fields: [] };
			var settings = $.extend({}, defaults, options || {});
			var url = "WeatherForecast";

			if (settings.fields.length > 0) {
				url += url.indexOf("?") == -1 ? "?" : "&";
				url += "fields=" + settings.fields.join();
			}

			return invoke.call(this, url, "post",
				{}
				, wether);
		},

	};
	$.proxies.weatherforecast = {
		defaultOptions: {},
		antiForgeryToken: defaultAntiForgeryTokenAccessor,



		get: function (options) {
			var defaults = { fields: [] };
			var settings = $.extend({}, defaults, options || {});
			var url = "WeatherForecast";

			if (settings.fields.length > 0) {
				url += url.indexOf("?") == -1 ? "?" : "&";
				url += "fields=" + settings.fields.join();
			}

			return invoke.call(this, url, "get",
				{}
			);
		},



		post: function (wether, options) {
			var defaults = { fields: [] };
			var settings = $.extend({}, defaults, options || {});
			var url = "WeatherForecast";

			if (settings.fields.length > 0) {
				url += url.indexOf("?") == -1 ? "?" : "&";
				url += "fields=" + settings.fields.join();
			}

			return invoke.call(this, url, "post",
				{}
				, wether);
		},

	};
}(jQuery));

