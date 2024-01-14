const { Pokemon, Type } = require('../db');

const postPokemon = async (req, res) => {
    try {
        const { name, types, image, hp, attack, defense, speed, weight, height } = req.body;
        
        const createdPokemon = await Pokemon.create({
            name,
            image,
            hp,
            attack,
            defense,
            speed,
            weight,
            height
        });
        
        
        const foundTypes = await Type.findAll({
            where: {
                name: types
            }
        });
        
        await createdPokemon.addType(foundTypes);
        
        return res.status(201).json(createdPokemon);

    } catch (error) {
        return res.status(500).json({ error: error.message });
    }
};

module.exports = postPokemon;