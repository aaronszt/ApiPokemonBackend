const { DataTypes } = require('sequelize');
// Exportamos una funcion que define el modelo
// Luego le injectamos la conexion a sequelize.
module.exports = (sequelize) => {
  // defino el modelo
  sequelize.define('pokemon', {
    id:{
      type:DataTypes.UUID,
      defaultValue : DataTypes.UUIDV1,
      primaryKey:true,
      allowNull:false
    },
    name: {
      type: DataTypes.STRING,
      allowNull: false,
      unique:true,
      set(value) { this.setDataValue('name', value.toLowerCase()) }
    },
    image:{
      type: DataTypes.STRING,
      allowNull:true
    },
    hp:{
      type: DataTypes.INTEGER,
      allowNull:false,
      validate: {
        min: {
          args: 1,
          msg: 'HP must be at least 1.',
        },
        max: {
          args: 200,
          msg: 'HP cannot exceed 200.',
        }
      }
    },
    attack:{
      type: DataTypes.INTEGER,
      allowNull:false,
      validate: {
        min: {
          args: 1,
          msg: 'Attack must be at least 1.',
        },
        max: {
          args: 200,
          msg: 'Attack cannot exceed 200.',
        }
      }
    },
    defense:{
      type: DataTypes.INTEGER,
      allowNull:false,
      validate: {
        min: {
          args: 1,
          msg: 'Defense must be at least 1.',
        },
        max: {
          args: 200,
          msg: 'Defense cannot exceed 200.',
        }
      }
    },
    speed:{
      type: DataTypes.INTEGER,
      allowNull:false,
      validate: {
        min: {
          args: 1,
          msg: 'Speed must be at least 1.',
        },
        max: {
          args: 200,
          msg: 'Speed cannot exceed 200.',
        }
      }
    },
    height:{
      type: DataTypes.INTEGER,
      allowNull:false,
      validate: {
        min: {
          args: 1,
          msg: 'Height must be at least 1.',
        },
        max: {
          args: 200,
          msg: 'Height cannot exceed 200.',
        }
      }
    },
    weight:{
      type: DataTypes.INTEGER,
      allowNull:false,
      validate: {
        min: {
          args: 1,
          msg: 'Wheight must be at least 1.',
        },
        max: {
          args: 1000,
          msg: 'Weight cannot exceed 1000.',
        }
      }
    },
    custom:{
      type: DataTypes.BOOLEAN,
      defaultValue: true,
      allowNull:false
    }
  },{timestamps: false});
};