const axios = require('axios');
const URL = `https://pokeapi.co/api/v2/pokemon`;
const { Pokemon, Type } = require('../db');

const getPokemonById = async (req, res) => {
    try {
        const { id } = req.params;

        if (isNaN(id)){
            const pokemonDB = await Pokemon.findOne({
                where:{ id: id },
                include: { model: Type, as: 'types', attributes: ['name'], through: { attributes: [] }
            }})

            const pokemonFix = {
                ...pokemonDB.toJSON(),
                types: pokemonDB.types.map(type => type.name)
              };
            
            return res.status(200).json(pokemonFix);
        }
        
        const { data } = await axios(`${URL}/${id}`);
        const { name, height, weight, stats, types, sprites } = data;
        
        if (!name) throw Error(`ID: ${id} Not found`);

        let hp, attack, defense, speed;
        stats.forEach((element) => { 
            switch (element.stat.name) {
                case 'hp':
                    hp = element.base_stat;
                    break;
                case 'attack':
                    attack = element.base_stat;
                    break;
                case 'defense':
                    defense = element.base_stat;
                    break;
                case 'speed':
                    speed = element.base_stat;
                    break;
            }
        });
        const image = sprites.other.home.front_default;
        const typeOne = types[0]?.type.name;
        const typeTwo = types[1]?.type.name;
        const type = [typeOne, typeTwo]
        const pokemon = {
            id,
            name,
            hp, 
            attack, 
            defense, 
            speed, 
            height, 
            weight,
            type,
            image
        }

        return res.status(200).json(pokemon);
        
    } catch (error) {
        return error.message.includes("ID")
        ? res.status(404).send(error.message)
        : res.status(500).send(error.message)
    }
}


module.exports = getPokemonById;