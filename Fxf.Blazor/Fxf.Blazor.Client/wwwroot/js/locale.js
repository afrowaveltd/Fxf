// wwwroot/js/locale.js
/**
 * Gets the list of preferred browser locales.
 * @returns {string[]} An array of locale strings, e.g., ["cs-CZ", "cs", "en-US", "en"].
 */
export function getBrowserLocales() {
	if (Array.isArray(navigator.languages) && navigator.languages.length > 0) {
		return navigator.languages;
	}
	return [navigator.language || "en"];
}

/**
 * Gets the browser's current time zone.
 * @returns {string|null} The IANA time zone name, or null if unavailable.
 */
export function getTimeZone() {
	try {
		return Intl.DateTimeFormat().resolvedOptions().timeZone || null;
	} catch {
		return null;
	}
}

/**
 * Sets a cookie with the specified name, value, and optional number of days until expiration.
 * @param {string} name - The name of the cookie.
 * @param {string} value - The value to store.
 * @param {number} [days=365] - Number of days until the cookie expires.
 */
function setCookie(name, value, days = 365) {
	const expires = new Date(Date.now() + days * 864e5).toUTCString();
	document.cookie = `${name}=${encodeURIComponent(value)}; expires=${expires}; path=/`;
}

/**
 * Gets the value of a cookie by name.
 * @param {string} name - The name of the cookie.
 * @returns {string|null} The cookie value, or null if not found.
 */
function getCookie(name) {
	const match = document.cookie.match(new RegExp('(?:^|; )' + name.replace(/([.$?*|{}()\[\]\\\/\+^])/g, '\\$1') + '=([^;]*)'));
	return match ? decodeURIComponent(match[1]) : null;
}

/**
 * Saves the user's preferred culture to a cookie named BlazorCulture.
 * @param {string} culture - The culture code to save (e.g., "en", "cs").
 */
export function savePreferredCulture(culture) {
	setCookie('BlazorCulture', culture);
}

/**
 * Retrieves the user's preferred culture from the BlazorCulture cookie, returning only the first two letters.
 * @returns {string|null} The two-letter culture code, or "en" if not set.
 */
export function getPreferredCulture() {
	let culture = getCookie('BlazorCulture');
	if (culture && culture.length > 2) {
		culture = culture.substring(0, 2);
	}
	if (!culture) {
		culture = "";
	}
	console.log(culture);
	return culture;
}