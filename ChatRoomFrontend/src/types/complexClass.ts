type Dictionary<TValue> = {
    [key: string | number]: TValue;
}

class ComplexComponent {
    message: string;
    isComplex: boolean;

    constructor(fromObject: object) {
        const errorMessage = ["property '", "' is missing in deserialized object"];

        if ("message" in fromObject) this.message = fromObject.message as string;
        else throw new Error(`${errorMessage[0]}message${errorMessage[1]}`);

        if ("isComplex" in fromObject) this.isComplex = fromObject.isComplex as boolean;
        else throw new Error(`${errorMessage[0]}isComplex${errorMessage[1]}`);
    }
}

export class ComplexClass {
    id: string;
    listOfDates: Date[];
    dictionaryOfDictionaries: Dictionary<Dictionary<ComplexComponent>>;

    constructor(fromObject: object) {
        const errorMessage = ["property '", "' is missing in deserialized object"];

        if ("id" in fromObject) this.id = fromObject.id as string;
        else throw new Error(`${errorMessage[0]}id${errorMessage[1]}`);

        if ("listOfDates" in fromObject) {
            this.listOfDates = (fromObject.listOfDates as string[]).map(date => new Date(date) as Date);
        }
        else throw new Error(`${errorMessage[0]}listOfDates${errorMessage[1]}`);

        if ("dictionaryOfDictionaries" in fromObject) {
            const constructedOuterDictionary: Dictionary<Dictionary<ComplexComponent>> = {};

            Object.entries(fromObject.dictionaryOfDictionaries as object).forEach((outerDictionary) => {
                const [outerDictionaryIdentifier, innerDictionaryObject] = outerDictionary;
                const constructedInnerDictionary: Dictionary<ComplexComponent> = {};

                Object.entries(innerDictionaryObject).forEach((innerKeyValue) => {
                    const [innerDictionaryIdentifier, complexComponentObject] = innerKeyValue;
                    const complexComponent = new ComplexComponent(complexComponentObject as object);
                    constructedInnerDictionary[innerDictionaryIdentifier] = complexComponent;
                });

                constructedOuterDictionary[outerDictionaryIdentifier] = constructedInnerDictionary;
            });

            this.dictionaryOfDictionaries = constructedOuterDictionary;
        }
        else throw new Error(`${errorMessage[0]}dictionaryOfDictionaries${errorMessage[1]}`);
    }
}