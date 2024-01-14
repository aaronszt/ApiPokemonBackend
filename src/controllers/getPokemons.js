const axios = require('axios');
const { Pokemon, Type } = require('../db');

const getPokemons = async (req, res) => {
    try {
        const URL = `https://pokeapi.co/api/v2/pokemon`
        let allApiPokemons = [];
        let nextUrl = URL;

        while (allApiPokemons.length < 60) {
            const response = await axios(nextUrl);
            const { results, next } = response.data;

            nextUrl = next;

            allApiPokemons.push(...results);

            if (!nextUrl) {
                break;
            }
        }

        const allDBPokemons = await Pokemon.findAll({
            include:[{
                model: Type, as: 'types', attributes: ['name'], through: { attributes: [] }
            }]
        });

        const dbPokemonsMapped = allDBPokemons.map((dbPokemon) => {
            return {
                ...dbPokemon.toJSON(),
                types: dbPokemon.types.map((type) => type.name)
            };
        });

        if (allApiPokemons.length > 0) {
            const pokemonDataPromises = allApiPokemons.map(async (pokemon) => {
                const response = await axios.get(pokemon.url);
                return response.data;
            });

            const apiPokemonDataList = await Promise.all(pokemonDataPromises);

            const pokemonApi = apiPokemonDataList.map(({name, height, weight, stats, types, sprites, id}) => {
                const hp = stats[0].base_stat;
                const attack = stats[1].base_stat;
                const defense = stats[2].base_stat;
                const speed = stats[5].base_stat;
                const typeOne = types[0]?.type.name;
                const typeTwo = types[1]?.type.name;
                const typeNames = [typeOne,typeTwo];
                const image = sprites.other.home.front_default;
                return {
                    id,
                    name,
                    hp,
                    attack,
                    defense,
                    speed,
                    height,
                    weight,
                    types: typeNames,
                    image
                };
            })

            const combinedPokemons = [...dbPokemonsMapped, ...pokemonApi];

            return res.status(200).json(combinedPokemons);
        }
    } catch (error) {
        return res.status(500).json({ error: error.message })
    }
}

module.exports = getPokemons;