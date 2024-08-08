const lengthPattern = (min: number, max: number) => `^.{${min},${max}}$`;
const maxLengthPattern = (max: number) => lengthPattern(0, max);
const minLengthPattern = (min: number) => `^.{${min},}$`;

export { lengthPattern, maxLengthPattern, minLengthPattern };
