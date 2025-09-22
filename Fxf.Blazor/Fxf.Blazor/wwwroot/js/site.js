const setTheme = async (theme) => {
    if (!theme || theme == "undefined") {
        console.error("theme is not defined");
        return;
    }
    const path = `/api/theme/set/${theme}`;
    let response = await fetch(path);
    console.log(response.status);
    if (response.status == 200) {
        location.reload();
        return;
    }
    if (response.status == 404) {
        console.error("Wrong URL");
        return;
    }

    let result = await response.json();
    console.error(result.error);
    return;
}

const changeLanguage = async (theme) => {
    if (!theme || theme == "undefined") {
        console.error("theme is not defined");
        return;
    }
    const path = `/api/language/set/${theme}`;
    let response = await fetch(path);
    console.log(response.status);
    if (response.status == 200) {
        location.reload();
        return;
    }
    if (response.status == 404) {
        console.error("Wrong URL");
        return;
    }

    let result = await response.json();
    console.error(result.error);
    return;
}