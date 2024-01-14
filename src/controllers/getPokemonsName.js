const axios = require('axios');
const {getAllPokemons} = require('../handler/getAllPokemons');
const { Pokemon, Type } = require("../db");


const getPokemonsName = async (request, response) => {
    try {
        
        let { name } = request.query;
        name = name.toLowerCase();

        if (!name) throw Error(`The pokemon with ${name} does not exist`);

        const searchPokeNameDB = await Pokemon.findOne({
            where: { name: name },
            include: { model: Type, as: 'types', attributes: ['name'], through: { attributes: [] } }
        });

        if (searchPokeNameDB) {
            const pokemonFix = {
                ...searchPokeNameDB.toJSON(),
                types: searchPokeNameDB.types.map(type => type.name),
                hooks:{
                    beforeSave:(pokemon, options) => {
                      pokemon.name = pokemon.name.toLowerCase();
                    }
                  }
            };

            return response.status(200).json([pokemonFix]); 
        }


        const allApiPokemons = await getAllPokemons();
        
        
        const matchingPokemons = allApiPokemons.filter(pokemon => pokemon.name.includes(name));
        
        if (matchingPokemons.length > 0) {
            
            const pokemonDataPromises = matchingPokemons.map(async (pokemon) => {
                const response = await axios.get(pokemon.url);
                return response.data;
            });
            
            const pokemonDataList = await Promise.all(pokemonDataPromises);

            let pokemons = pokemonDataList.map(({ name, height, weight, stats, types, id, sprites }) => {
                const { base_stat: hp } = stats.find((stat) => stat.stat.name === "hp");
                    const { base_stat: attack } = stats.find((stat) => stat.stat.name === "attack");
                    const { base_stat: defense } = stats.find((stat) => stat.stat.name === "defense");
                    const { base_stat: speed } = stats.find((stat) => stat.stat.name === "speed");
                    const typeOne = types[0]?.type.name;
                    const typeTwo = types[1]?.type.name;
                    const typeId = [typeOne, typeTwo];
                    const image = sprites.other.home.front_default;
                return {
                    id,
                    name,
                    image,
                    hp,
                    attack,
                    defense,
                    speed,
                    height,
                    weight,
                    types:typeId
                };
            });

        
            

            return response.status(200).json(pokemons);
        } 
        else {
            return response.status(404).send("No matching Pokemon found");
        }
    } catch (error) {
        return response.status(500).send(error.message);
    }
};

module.exports = getPokemonsName
