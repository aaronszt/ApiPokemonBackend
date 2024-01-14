const URL = `https://pokeapi.co/api/v2/pokemon`;
const axios = require ("axios");

const getAllPokemons = async (url = URL, maxCount = 60) => {
    try {
        const response = await axios.get(url);
        const { results, next } = response.data;
        const allPokemons = results;

        if (maxCount <= 0) {
            return [];
        }
        if (next) { 
            const nextPagePokemons = await getAllPokemons(next, maxCount - allPokemons.length);
            return [...allPokemons, ...nextPagePokemons];
        }

        return allPokemons;
    } catch (error) {
        throw new Error(error.message);
    }
};

module.exports = {getAllPokemons}