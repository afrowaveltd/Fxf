namespace Fxf.Blazor.Services;

/// <summary>
/// Provides language and localization utilities for the application, including: language metadata,
/// discovery of available locale files, loading and saving translation dictionaries, and helper operations.
/// </summary>
public class LanguageService(IConfiguration configuration, IStringLocalizer<LanguageService> t) : ILanguageService
{
	private readonly IConfiguration _configuration = configuration;
	private static readonly string _localesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0], "Locales");
	private static readonly string _clientLocalesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0], "LocalesClient");
	private static readonly string _oldTranslationPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "old.json");
	private static readonly string _oldClientTranslationPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "old_client.json");
	private readonly IStringLocalizer<LanguageService> _t = t;

	private readonly List<Language> _languages =
	[
		 new Language { Code = "aa", Name = "Afar", Native = "Afar" },
						  new Language { Code = "ab", Name = "Abkhazian", Native = "Аҧсуа" },
						  new Language { Code = "af", Name = "Afrikaans", Native = "Afrikaans" },
						  new Language { Code = "ak", Name = "Akan", Native = "Akana" },
						  new Language { Code = "am", Name = "Amharic", Native = "አማርኛ" },
						  new Language { Code = "an", Name = "Aragonese", Native = "Aragonés" },
						  new Language { Code = "ar", Name = "Arabic", Native = "العربية", Rtl = true },
						  new Language { Code = "as", Name = "Assamese", Native = "অসমীয়া" },
						  new Language { Code = "av", Name = "Avar", Native = "Авар" },
						  new Language { Code = "ay", Name = "Aymara", Native = "Aymar" },
						  new Language { Code = "az", Name = "Azerbaijani", Native = "Azərbaycanca" },
						  new Language { Code = "ba", Name = "Bashkir", Native = "Башҡорт" },
						  new Language { Code = "be", Name = "Belarusian", Native = "Беларуская" },
						  new Language { Code = "bg", Name = "Bulgarian", Native = "Български" },
						  new Language { Code = "bh", Name = "Bihari", Native = "भोजपुरी" },
						  new Language { Code = "bi", Name = "Bislama", Native = "Bislama" },
						  new Language { Code = "bm", Name = "Bambara", Native = "Bamanankan" },
						  new Language { Code = "bn", Name = "Bengali", Native = "বাংলা" },
						  new Language { Code = "bo", Name = "Tibetan", Native = "བོད་ཡིག / Bod skad" },
						  new Language { Code = "br", Name = "Breton", Native = "Brezhoneg" },
						  new Language { Code = "bs", Name = "Bosnian", Native = "Bosanski" },
						  new Language { Code = "ca", Name = "Catalan", Native = "Català" },
						  new Language { Code = "ce", Name = "Chechen", Native = "Нохчийн" },
						  new Language { Code = "ch", Name = "Chamorro", Native = "Chamoru" },
						  new Language { Code = "co", Name = "Corsican", Native = "Corsu" },
						  new Language { Code = "cr", Name = "Cree", Native = "Nehiyaw" },
						  new Language { Code = "cs", Name = "Czech", Native = "Česky" },
						  new Language { Code = "cu", Name = "Old Church Slavonic / Old Bulgarian", Native = "словѣньскъ / slověnĭskŭ" },
						  new Language { Code = "cv", Name = "Chuvash", Native = "Чăваш" },
						  new Language { Code = "cy", Name = "Welsh", Native = "Cymraeg" },
						  new Language { Code = "da", Name = "Danish", Native = "Dansk" },
						  new Language { Code = "de", Name = "German", Native = "Deutsch" },
						  new Language { Code = "dv", Name = "Divehi", Native = "ދިވެހިބަސް", Rtl = true },
						  new Language { Code = "dz", Name = "Dzongkha", Native = "ཇོང་ཁ" },
						  new Language { Code = "ee", Name = "Ewe", Native = "Ɛʋɛ" },
						  new Language { Code = "el", Name = "Greek", Native = "Ελληνικά" },
						  new Language { Code = "en", Name = "English", Native = "English" },
						  new Language { Code = "eo", Name = "Esperanto", Native = "Esperanto" },
						  new Language { Code = "es", Name = "Spanish", Native = "Español" },
						  new Language { Code = "et", Name = "Estonian", Native = "Eesti" },
						  new Language { Code = "eu", Name = "Basque", Native = "Euskara" },
						  new Language { Code = "fa", Name = "Persian", Native = "فارسی", Rtl = true },
						  new Language { Code = "ff", Name = "Peul", Native = "Fulfulde" },
						  new Language { Code = "fi", Name = "Finnish", Native = "Suomi" },
						  new Language { Code = "fj", Name = "Fijian", Native = "Na Vosa Vakaviti" },
						  new Language { Code = "fo", Name = "Faroese", Native = "Føroyskt" },
						  new Language { Code = "fr", Name = "French", Native = "Français" },
						  new Language { Code = "fy", Name = "West Frisian", Native = "Frysk" },
						  new Language { Code = "ga", Name = "Irish", Native = "Gaeilge" },
						  new Language { Code = "gd", Name = "Scottish Gaelic", Native = "Gàidhlig" },
						  new Language { Code = "gl", Name = "Galician", Native = "Galego" },
						  new Language { Code = "gn", Name = "Guarani", Native = "Avañe'ẽ" },
						  new Language { Code = "gu", Name = "Gujarati", Native = "ગુજરાતી" },
						  new Language { Code = "gv", Name = "Manx", Native = "Gaelg" },
						  new Language { Code = "ha", Name = "Hausa", Native = "هَوُسَ", Rtl = true },
						  new Language { Code = "he", Name = "Hebrew", Native = "עברית", Rtl = true },
						  new Language { Code = "hi", Name = "Hindi", Native = "हिन्दी" },
						  new Language { Code = "ho", Name = "Hiri Motu", Native = "Hiri Motu" },
						  new Language { Code = "hr", Name = "Croatian", Native = "Hrvatski" },
						  new Language { Code = "ht", Name = "Haitian", Native = "Krèyol ayisyen" },
						  new Language { Code = "hu", Name = "Hungarian", Native = "Magyar" },
						  new Language { Code = "hy", Name = "Armenian", Native = "Հայերեն" },
						  new Language { Code = "hz", Name = "Herero", Native = "Otsiherero" },
						  new Language { Code = "ia", Name = "Interlingua", Native = "Interlingua" },
						  new Language { Code = "id", Name = "Indonesian", Native = "Bahasa Indonesia" },
						  new Language { Code = "ie", Name = "Interlingue", Native = "Interlingue" },
						  new Language { Code = "ig", Name = "Igbo", Native = "Igbo" },
						  new Language { Code = "ii", Name = "Sichuan Yi", Native = "ꆇꉙ / 四川彝语" },
						  new Language { Code = "ik", Name = "Inupiak", Native = "Iñupiak" },
						  new Language { Code = "io", Name = "Ido", Native = "Ido" },
						  new Language { Code = "is", Name = "Icelandic", Native = "Íslenska" },
						  new Language { Code = "it", Name = "Italian", Native = "Italiano" },
						  new Language { Code = "iu", Name = "Inuktitut", Native = "ᐃᓄᒃᑎᑐᑦ" },
						  new Language { Code = "ja", Name = "Japanese", Native = "日本語" },
						  new Language { Code = "jv", Name = "Javanese", Native = "Basa Jawa" },
						  new Language { Code = "ka", Name = "Georgian", Native = "ქართული" },
						  new Language { Code = "kg", Name = "Kongo", Native = "KiKongo" },
						  new Language { Code = "ki", Name = "Kikuyu", Native = "Gĩkũyũ" },
						  new Language { Code = "kj", Name = "Kuanyama", Native = "Kuanyama" },
						  new Language { Code = "kk", Name = "Kazakh", Native = "Қазақша" },
						  new Language { Code = "kl", Name = "Greenlandic", Native = "Kalaallisut" },
						  new Language { Code = "km", Name = "Cambodian", Native = "ភាសាខ្មែរ" },
						  new Language { Code = "kn", Name = "Kannada", Native = "ಕನ್ನಡ" },
						  new Language { Code = "ko", Name = "Korean", Native = "한국어" },
						  new Language { Code = "kr", Name = "Kanuri", Native = "Kanuri" },
						  new Language { Code = "ks", Name = "Kashmiri", Native = "कश्मीरी / كشميري", Rtl = true },
						  new Language { Code = "ku", Name = "Kurdish", Native = "Kurdî / كوردی", Rtl = true },
						  new Language { Code = "kv", Name = "Komi", Native = "Коми" },
						  new Language { Code = "kw", Name = "Cornish", Native = "Kernewek" },
						  new Language { Code = "ky", Name = "Kirghiz", Native = "Kırgızca / Кыргызча" },
						  new Language { Code = "la", Name = "Latin", Native = "Latina" },
						  new Language { Code = "lb", Name = "Luxembourgish", Native = "Lëtzebuergesch" },
						  new Language { Code = "lg", Name = "Ganda", Native = "Luganda" },
						  new Language { Code = "li", Name = "Limburgian", Native = "Limburgs" },
						  new Language { Code = "ln", Name = "Lingala", Native = "Lingála" },
						  new Language { Code = "lo", Name = "Laotian", Native = "ລາວ / Pha xa lao" },
						  new Language { Code = "lt", Name = "Lithuanian", Native = "Lietuvių" },
						  new Language { Code = "lu", Name = "Luba-Katanga", Native = "Tshiluba" },
						  new Language { Code = "lv", Name = "Latvian", Native = "Latviešu" },
						  new Language { Code = "mg", Name = "Malagasy", Native = "Malagasy" },
						  new Language { Code = "mh", Name = "Marshallese", Native = "Kajin Majel / Ebon" },
						  new Language { Code = "mi", Name = "Maori", Native = "Māori" },
						  new Language { Code = "mk", Name = "Macedonian", Native = "Македонски" },
						  new Language { Code = "ml", Name = "Malayalam", Native = "മലയാളം" },
						  new Language { Code = "mn", Name = "Mongolian", Native = "Монгол" },
						  new Language { Code = "mo", Name = "Moldovan", Native = "Moldovenească" },
						  new Language { Code = "mr", Name = "Marathi", Native = "मराठी" },
						  new Language { Code = "ms", Name = "Malay", Native = "Bahasa Melayu" },
						  new Language { Code = "mt", Name = "Maltese", Native = "bil-Malti" },
						  new Language { Code = "my", Name = "Burmese", Native = "မြန်မာစာ" },
						  new Language { Code = "na", Name = "Nauruan", Native = "Dorerin Naoero" },
						  new Language { Code = "nb", Name = "Norwegian Bokmål", Native = "Norsk bokmål" },
						  new Language { Code = "nd", Name = "North Ndebele", Native = "Sindebele" },
						  new Language { Code = "ne", Name = "Nepali", Native = "नेपाली" },
						  new Language { Code = "ng", Name = "Ndonga", Native = "Oshiwambo" },
						  new Language { Code = "nl", Name = "Dutch", Native = "Nederlands" },
						  new Language { Code = "nn", Name = "Norwegian Nynorsk", Native = "Norsk nynorsk" },
						  new Language { Code = "no", Name = "Norwegian", Native = "Norsk" },
						  new Language { Code = "nr", Name = "South Ndebele", Native = "isiNdebele" },
						  new Language { Code = "nv", Name = "Navajo", Native = "Diné bizaad" },
						  new Language { Code = "ny", Name = "Chichewa", Native = "Chi-Chewa" },
						  new Language { Code = "oc", Name = "Occitan", Native = "Occitan" },
						  new Language { Code = "oj", Name = "Ojibwa", Native = "ᐊᓂᔑᓈᐯᒧᐎᓐ / Anishinaabemowin" },
						  new Language { Code = "om", Name = "Oromo", Native = "Oromoo" },
						  new Language { Code = "or", Name = "Oriya", Native = "ଓଡ଼ିଆ" },
						  new Language { Code = "os", Name = "Ossetian / Ossetic", Native = "Иронау" },
						  new Language { Code = "pa", Name = "Panjabi / Punjabi", Native = "ਪੰਜਾਬੀ / पंजाबी / پنجابي" },
						  new Language { Code = "pb", Name = "Portuguese (Brazil)", Native = "Português (Brasil)"},
						  new Language { Code = "pi", Name = "Pali", Native = "Pāli / पाऴि" },
						  new Language { Code = "pl", Name = "Polish", Native = "Polski" },
						  new Language { Code = "ps", Name = "Pashto", Native = "پښتو", Rtl = true },
						  new Language { Code = "pt", Name = "Portuguese", Native = "Português" },
						  new Language { Code = "qu", Name = "Quechua", Native = "Runa Simi" },
						  new Language { Code = "rm", Name = "Raeto Romance", Native = "Rumantsch" },
						  new Language { Code = "rn", Name = "Kirundi", Native = "Kirundi" },
						  new Language { Code = "ro", Name = "Romanian", Native = "Română" },
						  new Language { Code = "ru", Name = "Russian", Native = "Русский" },
						  new Language { Code = "rw", Name = "Rwandi", Native = "Kinyarwandi" },
						  new Language { Code = "sa", Name = "Sanskrit", Native = "संस्कृतम्" },
						  new Language { Code = "sc", Name = "Sardinian", Native = "Sardu" },
						  new Language { Code = "sd", Name = "Sindhi", Native = "सिनधि" },
						  new Language { Code = "se", Name = "Northern Sami", Native = "Sámegiella" },
						  new Language { Code = "sg", Name = "Sango", Native = "Sängö" },
						  new Language { Code = "sh", Name = "Serbo-Croatian", Native = "Srpskohrvatski / Српскохрватски" },
						  new Language { Code = "si", Name = "Sinhalese", Native = "සිංහල" },
						  new Language { Code = "sk", Name = "Slovak", Native = "Slovenčina" },
						  new Language { Code = "sl", Name = "Slovenian", Native = "Slovenščina" },
						  new Language { Code = "sm", Name = "Samoan", Native = "Gagana Samoa" },
						  new Language { Code = "sn", Name = "Shona", Native = "chiShona" },
						  new Language { Code = "so", Name = "Somalia", Native = "Soomaaliga" },
						  new Language { Code = "sq", Name = "Albanian", Native = "Shqip" },
						  new Language { Code = "sr", Name = "Serbian", Native = "Српски" },
						  new Language { Code = "ss", Name = "Swati", Native = "SiSwati" },
						  new Language { Code = "st", Name = "Southern Sotho", Native = "Sesotho" },
						  new Language { Code = "su", Name = "Sundanese", Native = "Basa Sunda" },
						  new Language { Code = "sv", Name = "Swedish", Native = "Svenska" },
						  new Language { Code = "sw", Name = "Swahili", Native = "Kiswahili" },
						  new Language { Code = "ta", Name = "Tamil", Native = "தமிழ்" },
						  new Language { Code = "te", Name = "Telugu", Native = "తెలుగు" },
						  new Language { Code = "tg", Name = "Tajik", Native = "Тоҷикӣ" },
						  new Language { Code = "th", Name = "Thai", Native = "ไทย / Phasa Thai" },
						  new Language { Code = "ti", Name = "Tigrinya", Native = "ትግርኛ" },
						  new Language { Code = "tk", Name = "Turkmen", Native = "Туркмен / تركمن" },
						  new Language { Code = "tl", Name = "Filipino", Native = "Tagalog" },
						  new Language { Code = "tn", Name = "Tswana", Native = "Setswana" },
						  new Language { Code = "to", Name = "Tonga", Native = "Lea Faka-Tonga" },
						  new Language { Code = "tr", Name = "Turkish", Native = "Türkçe" },
						  new Language { Code = "ts", Name = "Tsonga", Native = "Xitsonga" },
						  new Language { Code = "tt", Name = "Tatar", Native = "Tatarça" },
						  new Language { Code = "tw", Name = "Twi", Native = "Twi" },
						  new Language { Code = "ty", Name = "Tahitian", Native = "Reo Mā`ohi" },
						  new Language { Code = "ug", Name = "Uyghur", Native = "Uyƣurqə / ئۇيغۇرچە" },
						  new Language { Code = "uk", Name = "Ukrainian", Native = "Українська" },
						  new Language { Code = "ur", Name = "Urdu", Native = "اردو", Rtl = true },
						  new Language { Code = "uz", Name = "Uzbek", Native = "Ўзбек" },
						  new Language { Code = "ve", Name = "Venda", Native = "Tshivenḓa" },
						  new Language { Code = "vi", Name = "Vietnamese", Native = "Tiếng Việt" },
						  new Language { Code = "vo", Name = "Volapük", Native = "Volapük" },
						  new Language { Code = "wa", Name = "Walloon", Native = "Walon" },
						  new Language { Code = "wo", Name = "Wolof", Native = "Wollof" },
						  new Language { Code = "xh", Name = "Xhosa", Native = "isiXhosa" },
						  new Language { Code = "yi", Name = "Yiddish", Native = "יִידיש", Rtl = true },
						  new Language { Code = "yo", Name = "Yoruba", Native = "Yorùbá" },
						  new Language { Code = "za", Name = "Zhuang", Native = "Cuengh / Tôô / 壮语" },
						  new Language { Code = "zh", Name = "Chinese", Native = "中文" },
						  new Language { Code = "zt", Name = "Chinese (Traditional)", Native = "中文（繁體）" },
						  new Language { Code = "zu", Name = "Zulu", Native = "isiZulu" }
	];

	/// <summary>
	/// Gets the list of all supported languages with their metadata.
	/// </summary>
	/// <returns>A list of <see cref="Language"/> objects representing all supported languages.</returns>
	public List<Language> GetAllLanguages() => _languages;

	/// <summary>
	/// Gets the display names of all supported languages, sorted alphabetically.
	/// </summary>
	/// <returns>A list of language display names.</returns>
	public List<string> GetLanguageNames()
	{
		return [.. _languages.Select(l => l.Name).OrderBy(l => l)];
	}

	/// <summary>
	/// Indicates whether the specified language is written right-to-left (RTL).
	/// </summary>
	/// <param name="code">The ISO language code (case-insensitive).</param>
	/// <returns><see langword="true"/> if the language is RTL; otherwise <see langword="false"/>.</returns>
	public bool IsRtl(string code)
	{
		if(string.IsNullOrWhiteSpace(code)) return false;

		var language = _languages.FirstOrDefault(l => l.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
		return language?.Rtl ?? false;
	}

	/// <summary>
	/// Retrieves language metadata by its code.
	/// </summary>
	/// <param name="code">The ISO language code to search for. Cannot be null, empty, or whitespace.</param>
	/// <returns>
	/// A <see cref="Response{T}"/> containing the language on success; otherwise a failed response.
	/// </returns>
	public Response<Language>? GetLanguageByCode(string code)
	{
		Response<Language> result = new();
		if(string.IsNullOrWhiteSpace(code))
		{
			Response<Language>.Fail(_t["Language code cannot be null or empty"].Value);
		}
		var language = _languages.FirstOrDefault(l => l.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
		if(language == null)
		{
			return Response<Language>.Fail($"{_t["Language code"]} {code} {_t["not found"].Value}");
		}
		return Response<Language>.Successful(language, _t["Language found successfully"].Value);
	}

	/// <summary>
	/// Discovers languages that have locale JSON files present in the configured directory.
	/// </summary>
	/// <param name="isFrontend">
	/// When true, checks the client (WebAssembly) locales directory; otherwise the server locales directory.
	/// </param>
	/// <returns>
	/// A <see cref="Response{T}"/> with the list of <see cref="Language"/> entries whose files are present.
	/// </returns>
	public Response<List<Language>> GetRequiredLanguagesAsync(bool isFrontend = false)
	{
		string locPath = isFrontend ? _clientLocalesPath : _localesPath;
		try
		{
			if(!Directory.Exists(locPath))
			{
				return Response<List<Language>>.Fail(_t["Locales directory not found"].Value);
			}

			var languageFiles = Directory.GetFiles(locPath, "??.json");
			List<Language> requiredLanguages = [];

			foreach(var file in languageFiles)
			{
				var languageCode = System.IO.Path.GetFileNameWithoutExtension(file).ToLowerInvariant();

				if(languageCode.Length == 2 && languageCode.All(char.IsLetter))
				{
					var language = _languages.FirstOrDefault(l =>
						  l.Code.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

					if(language != null)
					{
						requiredLanguages.Add(language);
					}
				}
			}

			return Response<List<Language>>.Successful(requiredLanguages, $"Successfully retrieved {requiredLanguages.Count} languages");
		}
		catch(Exception ex)
		{
			return Response<List<Language>>.Fail($"Error retrieving languages: {ex.Message}");
		}
	}

	/// <summary>
	/// Resolves language metadata for a collection of ISO codes.
	/// </summary>
	/// <param name="languages">A list of ISO language codes.</param>
	/// <returns>
	/// A <see cref="Response{T}"/> with detailed <see cref="Language"/> entries for each provided
	/// code. Unknown codes are returned as placeholder <see cref="Language"/> objects that echo the
	/// code as name/native.
	/// </returns>
	public Response<List<Language>> GetSelectedLanguagesInfo(List<string> languages)
	{
		Response<List<Language>> result = new()
		{
			Data = []
		};
		foreach(var language in languages)
		{
			try
			{
				if(_languages.Where(s => s.Code == language).First() == null)
				{
					result.Data.Add(new Shared.Models.Language { Code = language, Name = language, Native = language, Rtl = false });
				}
				else
				{
					result.Data.Add(_languages.Where(s => s.Code == language).First());
				}
			}
			catch(Exception ex)
			{
				result.Data.Add(new Language { Code = language, Name = language, Native = language, Rtl = false });
			}
		}
		return result;
	}

	/// <summary>
	/// Asynchronously loads a language dictionary from a locale JSON file.
	/// </summary>
	/// <param name="code">Two-letter ISO language code (e.g., "en", "fr").</param>
	/// <param name="isFrontend">
	/// When true, reads from the client (WebAssembly) locales folder; otherwise from the server
	/// locales folder.
	/// </param>
	/// <returns>
	/// A <see cref="Response{T}"/> with the key/value dictionary on success. Returns a warning when
	/// the file is empty.
	/// </returns>
	public async Task<Response<Dictionary<string, string>>> GetDictionaryAsync(string code, bool isFrontend = false)
	{
		if(code == null || code.Length != 2)
		{
			return Response<Dictionary<string, string>>.Fail(_t["Invalid code"].Value);
		}
		var filePath = isFrontend
			 ? System.IO.Path.Combine(_clientLocalesPath, code.ToLowerInvariant() + ".json")
			 : System.IO.Path.Combine(_localesPath, code.ToLowerInvariant() + ".json");
		if(!File.Exists(filePath))
		{
			return Response<Dictionary<string, string>>.Fail(_t["Dictionary file not found"].Value);
		}
		try
		{
			var fileContext = await File.ReadAllTextAsync(filePath);
			var data = new Dictionary<string, string>();
			try
			{
				data = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonDocument.Parse(fileContext));
				if(data == null)
				{
					return Response<Dictionary<string, string>>.SuccessWithWarning(data!, _t["No data in the file"].Value);
				}
				if(data?.Count == 0)
				{
					return Response<Dictionary<string, string>>.SuccessWithWarning(data!, _t["The list is empty"].Value);
				}
				return Response<Dictionary<string, string>>.Successful(data!, code);
			}
			catch(Exception ex)
			{
				return Response<Dictionary<string, string>>.Fail(ex);
			}
		}
		catch(Exception ex)
		{
			return Response<Dictionary<string, string>>.Fail(ex);
		}
	}

	/// <summary>
	/// Asynchronously retrieves the last stored translation dictionary from a temporary file.
	/// </summary>
	/// <param name="isFrontend">
	/// When true, reads the client translation path; otherwise the server translation path.
	/// </param>
	/// <returns>
	/// A <see cref="Response{T}"/> with the last stored dictionary when available; otherwise a
	/// failed response.
	/// </returns>
	public async Task<Response<Dictionary<string, string>>> GetLastStored(bool isFrontend = false)
	{
		string oldPath = isFrontend ? _oldClientTranslationPath : _oldTranslationPath;

		if(!File.Exists(oldPath))
		{
			return Response<Dictionary<string, string>>.Fail(_t["not found"].Value);
		}
		try
		{
			string oldTranslationJson = await File.ReadAllTextAsync(oldPath);
			if(oldTranslationJson == null)
				return Response<Dictionary<string, string>>.Fail(_t["old Translation File is empty"].Value);
			if(oldTranslationJson.Length == 0)
				return Response<Dictionary<string, string>>.Fail(_t["old Translation File is empty"].Value);
			return new Response<Dictionary<string, string>>() { Data = JsonSerializer.Deserialize<Dictionary<string, string>>(oldTranslationJson) };
		}
		catch(Exception ex)
		{
			return Response<Dictionary<string, string>>.Fail(ex);
		}
	}

	/// <summary>
	/// Asynchronously retrieves translation dictionaries for all languages that have locale files present.
	/// </summary>
	/// <param name="isFrontend">
	/// When true, reads from the client (WebAssembly) locales folder; otherwise from the server
	/// locales folder.
	/// </param>
	/// <returns>
	/// A <see cref="Response{TranslationTree}"/> containing translations mapped per language.
	/// </returns>
	public async Task<Response<TranslationTree>> GetAllDictionariesAsync(bool isFrontend = false)
	{
		var languagesNeeded = TranslationsPresented(isFrontend);
		if(languagesNeeded != null)
		{
			var final = new TranslationTree();
			foreach(var language in languagesNeeded)
			{
				Response<Dictionary<string, string>> response = await GetDictionaryAsync(language, isFrontend);
				final.Translations[language] = response.Data ?? [];
			}
			return new Response<TranslationTree>() { Success = true, Data = final };
		}
		return Response<TranslationTree>.Fail(_t["No files in the folder"].Value);
	}

#pragma warning disable CS1584 // XML comment has a syntactically incorrect cref attribute.
#pragma warning disable CS1658 // Warning overrides error.

	/// <summary>
	/// Asynchronously saves a translation dictionary to a locale JSON file for the specified
	/// language code.
	/// </summary>
	/// <param name="code">The two-letter ISO language code.</param>
	/// <param name="data">The dictionary of key/value translations to save.</param>
	/// <param name="isFrontend">
	/// When true, saves to the client (WebAssembly) locales folder; otherwise to the server locales folder.
	/// </param>
	/// <returns>A <see cref="Response{Boolean}"/> indicating success or failure.</returns>
	public async Task<Response<bool>> SaveDictionaryAsync(string code, Dictionary<string, string> data, bool isFrontend = false)
#pragma warning restore CS1658 // Warning overrides error.
#pragma warning restore CS1584 // XML comment has a syntactically incorrect cref attribute.
	{
		try
		{
			if(code == null)
			{
				return Response<bool>.Fail(_t["Code cant be null"].Value);
			}
			if(code.Length != 2)
			{
				return Response<bool>.Fail(_t["Invalid code"].Value);
			}
			var toStore = data == null ? [] : data;
			string json = JsonSerializer.Serialize(toStore);
			var path = isFrontend ? System.IO.Path.Combine(_clientLocalesPath, code + ".json") : System.IO.Path.Combine(_localesPath, code + ".json");
			if(File.Exists(path))
			{
				try
				{
					string backupPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "fxf", code + '_' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak");
				}
				catch(Exception ex)
				{
					return Response<bool>.Fail($"Error creating backup: {ex.Message}");
				}
				File.Delete(path);
			}
			await (File.WriteAllTextAsync(path, json));
			return Response<bool>.Successful(true, _t["Successfully stored"].Value);
		}
		catch(Exception ex)
		{
			return Response<bool>.Fail($"Error: {ex.Message}");
		}
	}

#pragma warning disable CS1584 // XML comment has a syntactically incorrect cref attribute.
#pragma warning disable CS1658 // Warning overrides error.

	/// <summary>
	/// Asynchronously stores the provided translation dictionary as the last known translation
	/// snapshot (temporary file).
	/// </summary>
	/// <param name="data">The dictionary of translations to store.</param>
	/// <param name="isFrontend">
	/// When true, stores to the client (WebAssembly) temp location; otherwise to the server temp location.
	/// </param>
	/// <returns>A <see cref="Response{Boolean}"/> indicating success or failure.</returns>
	public async Task<Response<bool>> SaveOldTranslationAsync(Dictionary<string, string> data, bool isFrontend = false)
#pragma warning restore CS1658 // Warning overrides error.
#pragma warning restore CS1584 // XML comment has a syntactically incorrect cref attribute.
	{
		try
		{
			var toStore = data == null ? [] : data;
			string json = JsonSerializer.Serialize(toStore);
			var path = isFrontend ? _oldClientTranslationPath : _oldTranslationPath;
			await (File.WriteAllTextAsync(path, json));
			return Response<bool>.Successful(true, _t["old translations successfully stored"].Value);
		}
		catch(Exception ex)
		{
			return Response<bool>.Fail($"Error: {ex.Message}");
		}
	}

	/// <summary>
	/// Asynchronously saves all language dictionaries contained in a <see cref="TranslationTree"/>.
	/// </summary>
	/// <param name="translationTree">
	/// The translation tree containing all language dictionaries to save.
	/// </param>
	/// <param name="isFrontend">
	/// When true, saves to the client (WebAssembly) locales folder; otherwise to the server locales folder.
	/// </param>
	/// <returns>
	/// A <see cref="Response{T}"/> with a per-language success map (key = language code, value =
	/// success indicator).
	/// </returns>
	public async Task<Response<Dictionary<string, bool>>> SaveTranslationsAsync(TranslationTree translationTree, bool isFrontend = false)
	{
		var data = translationTree.Translations;
		var response = new Response<Dictionary<string, bool>>
		{
			Data = []
		};
		foreach(var entry in data)
		{
			string language = entry.Key;
			Dictionary<string, string> dictionary = entry.Value;
			var result = await SaveDictionaryAsync(language, dictionary, isFrontend);
			response.Data[language] = result.Success;
		}
		return response;
	}

	/// <summary>
	/// Gets the list of language codes for which translation JSON files are present.
	/// </summary>
	/// <param name="isFrontend">
	/// When true, scans the client (WebAssembly) locales folder; otherwise the server locales folder.
	/// </param>
	/// <returns>An array of ISO language codes.</returns>
	public string[] TranslationsPresented(bool isFrontend = false)
	{
		List<string> result = [];
		var languageFiles = Directory.GetFiles(isFrontend ? _clientLocalesPath : _localesPath, "??.json");
		foreach(var languageFile in languageFiles)
		{
			result.Add(System.IO.Path.GetFileNameWithoutExtension(languageFile).ToLowerInvariant());
		}
		return [.. result];
	}
}