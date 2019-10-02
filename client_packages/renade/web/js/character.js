var data = {
    "father": ["Benjamin", "Daniel"],
    "mother": ["Hannah", "Aubrey"],
    "eyebrowsM": ["None", "Balanced"],
    "eyebrowsF": ["None", "Balanced"],
    "beard": ["None", "Light Stubble"],
    "hairM": ["None", "Buzzcut"],
    "hairF": ["None", "Buzzcut"],
    "hairColor": ["Red", "Blue"],
    "eyeColor": ["Green", "Emerald"]
};

Vue.component('list', {
    template: '<div v-bind:id="id" class="list">\
    <i @click="left" class="left flaticon-left-arrow"></i>\
    <div>{{ values[index] }}</div>\
    <i @click="right" class="right flaticon-arrowhead-pointing-to-the-right"></i></div>',
    props: ['id', 'num'],
    data: function() {
        return {
            index: 0,
            values: this.num ? [-1, -0.1, -0.2, -0.3, -0.4, -0.5, -0.6, -0.7, -0.8, -0.9, 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1] : data[this.id],
        }
    },
    methods: {
        left: function(event) {
            this.index--
                if (this.index < 0) this.index = 0
            this.send()
        },
        right: function(event) {
            this.index++
                if (this.index == this.values.length) this.index = 0
            this.send()
        },
        send: function() {
            var value = this.num ? this.values[this.index] : this.index
            mp.trigger('editorList', this.id, Number(value))
        }
    }
})

var editor = new Vue({
    el: ".editor",
    data: {
        active: true,
        gender: true,
        isSurgery: false,
    },
    methods: {
        genderSw: function(type) {
            if (type) {
                this.gender = true
                mp.trigger('characterGender', "Male")
            } else {
                this.gender = false
                mp.trigger('characterGender', "Female")
            }
        },
        save: function() {
            mp.trigger('characterSave')
        }
    }
});

$(function() {
    $(document).on('input', 'input[type="range"]', function(e) {
        let id = e.target.id;
        let val = e.target.value;
        $('output#' + id).html(val);
        mp.trigger('editorList', id, Number(val));
    });

    $('input[type=range]').rangeslider({
        polyfill: false,
    });

    $('#gendermale').on('click', function() {
        $('#genderfemale').removeClass('on');
        $('#gendermale').addClass('on');
        editor.genderSw(true);
    });

    $('#genderfemale').on('click', function() {
        $('#gendermale').removeClass('on');
        $('#genderfemale').addClass('on');
        editor.genderSw(false);
    });
});