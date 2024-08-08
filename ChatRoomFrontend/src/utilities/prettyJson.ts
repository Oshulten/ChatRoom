type FormattingCharacter = {
    character: string,
    tab: number,
    index?: number
}

export function prettyJson(data: object) {
    const json = JSON.stringify(data);
    let formattedJson = "";

    const formattingCharacters: FormattingCharacter[] = [
        {
            character: "{",
            tab: 1
        },
        {
            character: "[",
            tab: 1
        },
        {
            character: ",",
            tab: 0
        },
        {
            character: "}",
            tab: -1
        },
        {
            character: "]",
            tab: -1
        }];

    let index = 0;
    let tabDepth = 0;
    while (index < formattedJson.length) {
        const searchResult = formattingCharacters.map((formatter) => { return { ...formatter, index: json.indexOf(formatter.character, index) } });
        if (searchResult.every(result => result.index == -1)) break;
        const closestResult = searchResult.sort((a, b) => a.index - b.index)[0];
        tabDepth += closestResult.tab;
        const line = json.slice(index, closestResult.index);
        formattedJson += `\n${"\t".repeat(tabDepth)}${line}\n`;
        index = closestResult.index;
    }
    return formattedJson;
}